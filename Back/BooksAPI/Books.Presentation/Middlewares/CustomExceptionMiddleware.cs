using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using Books.Core.Dtos.Read;

namespace Books.Presentation.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILocalizationService _localizationService;

    public CustomExceptionMiddleware(RequestDelegate next, ILocalizationService localizationService)
    {
        _next = next;
        _localizationService = localizationService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BookException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, BookException exception)
    {
        var language = GetLanguageFromRequest(context);
        var statusCode = GetStatusCodeForExceptionType(exception.ExceptionType);
        
        var errorResponse = new ErrorResponseDto.ErrorResponse
        {
            Success = false,
            Code = statusCode,
            Message = _localizationService.GetLocalizedString(exception.Message, language),
            Ex = exception.ExceptionType.ToString(),
            RequestDate = DateTime.UtcNow,
            Ticks = DateTime.UtcNow.Ticks
        };
        context.Response.StatusCode = statusCode;

        await WriteResponseAsync(context, errorResponse);
        
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(jsonResponse);
    }

    private static int GetStatusCodeForExceptionType(ExceptionType exceptionType)
    {
        return exceptionType switch
        {
            ExceptionType.InvalidToken => (int)HttpStatusCode.BadRequest,
            ExceptionType.InvalidRefreshToken => (int)HttpStatusCode.BadRequest,
            ExceptionType.InvalidCredentials => (int)HttpStatusCode.BadRequest,
            ExceptionType.UserNotFound => (int)HttpStatusCode.NotFound,
            ExceptionType.NullCredentials => (int)HttpStatusCode.BadRequest,
            ExceptionType.InvalidRequest => (int)HttpStatusCode.BadRequest,
            ExceptionType.PasswordMismatch => (int)HttpStatusCode.BadRequest,
            ExceptionType.EmailAlreadyConfirmed => (int)HttpStatusCode.BadRequest,
            ExceptionType.EmailNotConfirmed => (int)HttpStatusCode.BadRequest,
            ExceptionType.EmailAlreadyExists => (int)HttpStatusCode.BadRequest,
            ExceptionType.CredentialsAlreadyExists => (int)HttpStatusCode.BadRequest,
            ExceptionType.NotFound => (int)HttpStatusCode.NotFound,
            ExceptionType.UnauthorizedAccess => (int)HttpStatusCode.Unauthorized,
            ExceptionType.Forbidden => (int)HttpStatusCode.Forbidden,
            ExceptionType.BadRequest => (int)HttpStatusCode.BadRequest,
            ExceptionType.Conflict => (int)HttpStatusCode.Conflict,
            ExceptionType.InternalServerError => (int)HttpStatusCode.InternalServerError,
            ExceptionType.ServiceUnavailable => (int)HttpStatusCode.ServiceUnavailable,
            ExceptionType.OperationFailed => (int)HttpStatusCode.BadRequest,
            ExceptionType.DatabaseError => (int)HttpStatusCode.InternalServerError,
            ExceptionType.Critical => (int)HttpStatusCode.InternalServerError,
            _ => (int)HttpStatusCode.InternalServerError,
        };
    }
    
    private async Task WriteResponseAsync(HttpContext context, ErrorResponseDto.ErrorResponse errorResponse)
    {
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(jsonResponse);
    }

    private string GetLanguageFromRequest(HttpContext context)
    {
        return context.Request.Query["lang"].ToString().ToLower() switch
        {
            "ru" => "ru",
            "az" => "az",
            _ => context.Request.Headers["Accept-Language"].ToString().Split(',')[0].ToLower() switch
            {
                "ru" => "ru",
                "az" => "az",
                _ => "en"
            }
        };
    }
}
