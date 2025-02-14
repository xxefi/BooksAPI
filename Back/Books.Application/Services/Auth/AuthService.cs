using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Constants;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using DeviceDetectorNET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using static BCrypt.Net.BCrypt;

namespace Books.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBlackListedService _blackListedService;

    public AuthService(IMapper mapper,IUserService userService,
        ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IBlackListedService blackListedService)
    {
        _mapper = mapper;
        _userService = userService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _blackListedService = blackListedService;
    }
    
    public HttpContext httpContext => _httpContextAccessor.HttpContext;
    public async Task<AccessInfoDto> LoginAsync(LoginDto loginDto)
    {
        if (string.IsNullOrEmpty(loginDto?.Email) || string.IsNullOrEmpty(loginDto?.Password))
            throw new BookException(ExceptionType.InvalidRequest, "EmailAndPasswordCannotBeEmpty");
        
        if (!Regex.IsMatch(loginDto.Email, ValidationConstants.EmailRegex))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailFormat");
        
        var user = await _userService.GetUserByEmailAsync(loginDto.Email);
        var userCredentials = await _userService.GetUserCredentialsByIdAsync(user.Id);
        
        var isTokenBlackListed = await _blackListedService.IsBlackListedAsync(userCredentials.RefreshToken);
        if (isTokenBlackListed)
            throw new BookException(ExceptionType.InvalidRefreshToken, "TokenIsBlacklisted");
        
        if (!Verify(loginDto.Password, userCredentials.Password))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailOrPassword");
        
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        
        if (!string.IsNullOrEmpty(user.RefreshToken) && user.RefreshTokenExpiryTime > DateTime.UtcNow)
            throw new BookException(ExceptionType.InvalidRefreshToken, "RefreshTokenAlreadyExists");
        

        var updatedUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
        };
        
        await _userService.UpdateUserCredentialsAsync(updatedUser.UserId, updatedUser);
        
        httpContext.Response.Cookies.Append("accessToken", accessToken, cookieOptions);
        httpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

        return new AccessInfoDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

    }

    public async Task<AccessInfoDto> RefreshTokenAsync()
    {
        var accessToken = httpContext.Request.Cookies["accessToken"];
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new BookException(ExceptionType.InvalidRefreshToken, "MissingTokens");

        var userId = GetUserIdFromAccessToken(accessToken);
        var user = await _userService.GetUserCredentialsByIdAsync(userId);
        
        var isTokenBlacklisted = await _blackListedService.IsBlackListedAsync(refreshToken);
        if (isTokenBlacklisted)
            throw new BookException(ExceptionType.InvalidRefreshToken, "TokenIsBlacklisted");
        
        if (user.RefreshToken != refreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");
        
        if (!string.IsNullOrEmpty(user.RefreshToken) && user.RefreshTokenExpiryTime > DateTime.UtcNow)
            throw new BookException(ExceptionType.InvalidRefreshToken, "RefreshTokenAlreadyExists");
        
        var newAccessToken = _tokenService.GenerateAccessToken(_mapper.Map<UserDto>(user));
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        var updateUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime
        };
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        
        await _userService.UpdateUserCredentialsAsync(updateUser.UserId, updateUser);
        httpContext.Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);
        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);
        
        return new AccessInfoDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };

    }

    public async Task<bool> LogoutAsync()
    {
        var accessToken = httpContext.Request.Cookies["accessToken"];
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new BookException(ExceptionType.InvalidRefreshToken, "MissingTokens");

        var userId = GetUserIdFromAccessToken(accessToken);
        var user = await _userService.GetUserCredentialsByIdAsync(userId);
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        var deviceDetector = new DeviceDetector(userAgent);
        deviceDetector.Parse();
    
        var deviceInfo = deviceDetector.GetDeviceName();

        var newBlackList = new CreateBlackListedDto
        {
            IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            DeviceInfo = deviceInfo, 
            UserAgent = userAgent,
            Token = refreshToken
        };
        await _blackListedService.AddToBlackListAsync(newBlackList);
        
        if (user.RefreshToken != refreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");
        
        var updatedUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = null,
            RefreshTokenExpiryTime = DateTime.UtcNow 
        };
        httpContext.Response.Cookies.Delete("accessToken");
        httpContext.Response.Cookies.Delete("refreshToken");
        await _userService.UpdateUserCredentialsAsync(updatedUser.UserId, updatedUser);
        return true;
    }
    
    private Guid GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ReadToken(accessToken) as JwtSecurityToken
            ?? throw new BookException(ExceptionType.InvalidToken, "InvalidAccessToken");
        
        var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim);
            
        return userId;
    }
}