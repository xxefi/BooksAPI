using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Auth.Application.Exceptions;
using Auth.Core.Abstractions.Services;
using Auth.Core.Constants;
using Auth.Core.Dtos.Auth;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Microsoft.AspNetCore.Http;
using static BCrypt.Net.BCrypt;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IBlackListedService _blackListedService;
    private readonly IUserActiveSessionsService _userActiveSessionsService;

    public AuthService(IUserService userService,
        ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        IBlackListedService blackListedService, IUserActiveSessionsService userActiveSessionsService)
    {
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
            throw new AuthException(ExceptionType.InvalidRequest, "EmailAndPasswordCannotBeEmpty");
        
        if (!Regex.IsMatch(loginDto.Email, ValidationConstants.EmailRegex))
            throw new AuthException(ExceptionType.InvalidRequest, "InvalidEmailFormat");
        
        var user = await _userService.GetUserByEmailAsync(loginDto.Email);
        var userCredentials = await _userService.GetUserCredentialsByIdAsync(user.Id);
        
        
        if (!Verify(loginDto.Password, userCredentials.Password))
            throw new AuthException(ExceptionType.InvalidRequest, "InvalidEmailOrPassword");
        
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        var deviceInfo = httpContext.Request.Headers["User-Agent"].ToString();
        
        var createUserActiveSessionDto = new CreateUserActiveSessionDto
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
            DeviceInfo = deviceInfo
        };
        
        await _userActiveSessionsService.AddUserActiveSessionAsync(createUserActiveSessionDto);

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
            throw new AuthException(ExceptionType.InvalidRefreshToken, "MissingTokens");

        /*var userId = GetUserIdFromAccessToken(accessToken);
        var userActiveSession = await _userActiveSessionsService.GetUserActiveSessionAsync(userId);

        if (userActiveSession == null || userActiveSession.RefreshToken != refreshToken)
            throw new AuthException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");*/

        var userId = GetUserIdFromAccessToken(accessToken);
        
        var blackList = new BlackListedDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        var isTokenBlacklisted = await _blackListedService.IsBlackListedAsync(blackList);
        if (isTokenBlacklisted)
            throw new AuthException(ExceptionType.InvalidRefreshToken, "TokenIsBlacklisted");
        
        var user = await _userService.GetUserByIdAsync(userId);
        var userActiveSession = await _userActiveSessionsService.GetUserActiveSessionAsync(userId);

        if (userActiveSession.RefreshToken != refreshToken)
            throw new AuthException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");

        if (userActiveSession.RefreshTokenExpiryTime < DateTime.UtcNow)
            throw new AuthException(ExceptionType.InvalidRefreshToken, "RefreshTokenExpired");
        
        var newAccessToken = _tokenService.GenerateAccessToken(user!);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        var deviceInfo = httpContext.Request.Headers.UserAgent.ToString();
        
        var updateUserActiveSessionDto = new UpdateUserActiveSessionDto
        {
            Id = userActiveSession.Id,
            UserId = user.Id,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryDate = refreshTokenExpiryTime,
            DeviceInfo = deviceInfo
        };
        var newBlackList = new CreateBlackListedDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
        };
        await _blackListedService.AddToBlackListAsync(newBlackList);
        await _userActiveSessionsService.UpdateUserActiveSessionAsync(updateUserActiveSessionDto);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };
        
        httpContext.Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);
        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);
        
        return new AccessInfoDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        };

    }

    public async Task<bool> LogoutAsync(string accessToken, string refreshToken)
    {
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            throw new AuthException(ExceptionType.InvalidRefreshToken, "MissingTokens");

        var userId = GetUserIdFromAccessToken(accessToken);
        var userActiveSession = await _userActiveSessionsService.GetUserActiveSessionAsync(userId);

        if (userActiveSession == null || userActiveSession.RefreshToken != refreshToken)
            throw new AuthException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");

        var deviceInfo = httpContext.Request.Headers.UserAgent.ToString();

        var newBlackList = new CreateBlackListedDto
        {
            IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(),
            DeviceInfo = deviceInfo,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        await _blackListedService.AddToBlackListAsync(newBlackList);
        await _userActiveSessionsService.DeleteUserActiveSessionAsync(userActiveSession.Id);

        httpContext.Response.Cookies.Delete("accessToken");
        httpContext.Response.Cookies.Delete("refreshToken");

        return true;
    }
    
    public Guid GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ReadToken(accessToken) as JwtSecurityToken
            ?? throw new AuthException(ExceptionType.InvalidToken, "InvalidAccessToken");
        
        var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim!);
        return userId;
    }
}