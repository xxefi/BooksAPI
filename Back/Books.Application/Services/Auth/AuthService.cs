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
    private readonly IUserActiveSessionsService _userActiveSessionsService;

    public AuthService(IMapper mapper,IUserService userService,
        ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        IBlackListedService blackListedService, IUserActiveSessionsService userActiveSessionsService)
    {
        _mapper = mapper;
        _userService = userService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _blackListedService = blackListedService;
        _userActiveSessionsService = userActiveSessionsService;
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
        
        if (!Verify(loginDto.Password, userCredentials.Password))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailOrPassword");
        
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        var deviceInfo = httpContext.Request.Headers["User-Agent"].ToString();
        
        var createUserDeviceTokenDto = new CreateUserDeviceTokenDto
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
            DeviceInfo = deviceInfo
        };
        
        await _userActiveSessionsService.AddUserDeviceTokenAsync(createUserDeviceTokenDto);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        
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
        var userDeviceToken = await _userActiveSessionsService.GetUserDeviceTokenAsync(userId);

        if (userDeviceToken == null || userDeviceToken.RefreshToken != refreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");
        
        var blackList = new BlackListedDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        var isTokenBlacklisted = await _blackListedService.IsBlackListedAsync(blackList);
        if (isTokenBlacklisted)
            throw new BookException(ExceptionType.InvalidRefreshToken, "TokenIsBlacklisted");
        
        var newAccessToken = _tokenService.GenerateAccessToken(_mapper.Map<UserDto>(userDeviceToken));
        
        
        var updateUserDeviceTokenDto = new UpdateUserDeviceTokenDto
        {
            Id = userDeviceToken.Id,
            UserId = userDeviceToken.UserId,
            AccessToken = newAccessToken,
            DeviceInfo = userDeviceToken.DeviceInfo
        };
        var newBlackList = new CreateBlackListedDto
        {
            AccessToken = accessToken,
            IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
            DeviceInfo = userDeviceToken.DeviceInfo,
            UserAgent = userDeviceToken.DeviceInfo
        };
        await _blackListedService.AddToBlackListAsync(newBlackList);
        await _userActiveSessionsService.UpdateUserDeviceTokenAsync(updateUserDeviceTokenDto);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        
        httpContext.Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);
        
        return new AccessInfoDto
        {
            AccessToken = newAccessToken,
        };

    }

    public async Task<bool> LogoutAsync()
    {
        var accessToken = httpContext.Request.Cookies["accessToken"];
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
            
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new BookException(ExceptionType.InvalidRefreshToken, "MissingTokens");

        var userId = GetUserIdFromAccessToken(accessToken);
        var userDeviceToken = await _userActiveSessionsService.GetUserDeviceTokenAsync(userId);

        if (userDeviceToken == null || userDeviceToken.RefreshToken != refreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");

        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

        var deviceDetector = new DeviceDetector(userAgent);
        deviceDetector.Parse();
        
        var deviceInfo = deviceDetector.GetDeviceName();

        var newBlackList = new CreateBlackListedDto
        {
            IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            DeviceInfo = deviceInfo, 
            UserAgent = userAgent,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        await _blackListedService.AddToBlackListAsync(newBlackList);

        await _userActiveSessionsService.DeleteUserDeviceTokenAsync(userDeviceToken.Id);

        httpContext.Response.Cookies.Delete("accessToken");
        httpContext.Response.Cookies.Delete("refreshToken");

        return true;
    }
    
    public Guid GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ReadToken(accessToken) as JwtSecurityToken
            ?? throw new BookException(ExceptionType.InvalidToken, "InvalidAccessToken");
        
        var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim!);
        return userId;
    }
}