using System.Text.Json;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Read;

namespace Books.Presentation.Middlewares;

public class BlackListMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public BlackListMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/swagger-ui"))
        {
            await _next(context);
            return;
        }
        using var scope = _scopeFactory.CreateScope();
        var blackListedService = scope.ServiceProvider.GetRequiredService<IBlackListedService>();
        var accessToken = context.Request.Cookies["accessToken"];
        var refreshToken = context.Request.Cookies["refreshToken"];
        
        if (!string.IsNullOrEmpty(accessToken) || !string.IsNullOrEmpty(refreshToken))
        {
            var blackList = new BlackListedDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            var isAccessTokenBlackListed = await blackListedService.IsBlackListedAsync(blackList);
            if (isAccessTokenBlackListed)
            {
                var response = new
                {
                    message = "Access token or refresh token is blacklisted"
                };
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                context.Response.Headers["WWW-Authenticate"] = "Bearer error=\"invalid_token\", error_description=\"Access token or refresh token is blacklisted\"";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }
        }
        await _next(context);
    }
}