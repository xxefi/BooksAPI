using System.IdentityModel.Tokens.Jwt;
using Books.Core.Abstractions.Services.Auth;
using YamlDotNet.Core.Tokens;

namespace Books.Presentation.Middlewares;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;
    
    public TokenRefreshMiddleware(RequestDelegate next)
        => _next = next;

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var accessToken = context.Request.Cookies["accessToken"];

        if (!string.IsNullOrEmpty(accessToken) && IsTokenExpired(accessToken))
        {
            var refreshToken = context.Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var newTokens = await authService.RefreshTokenAsync();
                context.Response.Cookies.Append("accessToken", newTokens.AccessToken, new CookieOptions{ HttpOnly = true, Secure = true });
                context.Response.Cookies.Append("refreshToken", newTokens.RefreshToken, new CookieOptions { HttpOnly = true, Secure = true });
            }
        }
        await _next(context);
    }
    private bool IsTokenExpired(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        return jwtToken?.ValidTo < DateTime.UtcNow;
    }
}
