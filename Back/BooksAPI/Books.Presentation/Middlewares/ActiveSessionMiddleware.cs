using System.Text.Json;
using Books.Core.Abstractions.Services.Auth;

namespace Books.Presentation.Middlewares;

public class ActiveSessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public ActiveSessionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger") || 
            context.Request.Path.StartsWithSegments("/swagger-ui"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/api/Auth/Login") ||
            context.Request.Path.StartsWithSegments("/api/Auth/RefreshToken") ||
            context.Request.Path.StartsWithSegments("/api/Auth/Logout"))
        {
            await _next(context);
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var userActiveSessionsService = scope.ServiceProvider.GetRequiredService<IUserActiveSessionsService>();

        var accessToken = context.Request.Cookies["accessToken"];
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            await WriteJsonErrorResponse(context, StatusCodes.Status401Unauthorized, "Access or refresh token is missing");
            return;
        }

        var userId = authService.GetUserIdFromAccessToken(accessToken);
        if (userId == Guid.Empty)
        {
            await WriteJsonErrorResponse(context, StatusCodes.Status401Unauthorized, "Invalid or expired access token");
            return;
        }

        var isSessionActive = await userActiveSessionsService.IsUserSessionActiveAsync(userId);
        if (!isSessionActive)
        {
            await WriteJsonErrorResponse(context, StatusCodes.Status401Unauthorized, "User session is not active");
            return;
        }

        await _next(context);
    }

    private static async Task WriteJsonErrorResponse(HttpContext context, int statusCode, string message)
    {
        var response = new { message };
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
