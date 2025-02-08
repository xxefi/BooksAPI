using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Read;
using Books.Infrastructure.Context;

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
        var errorResponse = CreateErrorResponse(context, exception, language, statusCode);

        errorResponse.Message = _localizationService.GetLocalizedString(exception.Message, language);
        errorResponse.Ex = exception.ExceptionType.ToString();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        await context.Response.WriteAsync(jsonResponse);
    }
    
    private ErrorResponseDto.ErrorResponse CreateErrorResponse(HttpContext context, BookException exception, string language, int statusCode)
    {
        return new ErrorResponseDto.ErrorResponse
        {
            Success = false,
            Code = statusCode,
            Message = _localizationService.GetLocalizedString(exception.Message, language),
            Ex = exception.ExceptionType.ToString(),
            RequestDate = DateTime.UtcNow,
            Ticks = DateTime.UtcNow.Ticks,
            ActivityTraceId = context.TraceIdentifier,
            Details = new ErrorResponseDto.ErrorResponseDetails
            {
                Path = context.Request.Path,
                HttpMethod = context.Request.Method,
            },
        };
    }

    private int GetStatusCodeForExceptionType(ExceptionType exceptionType)
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
    private string GetLanguageFromRequest(HttpContext context)
    {
        string defaultLang = "en";
        var queryLang = context.Request.Query["lang"].ToString().ToLower();
        var headerLang = context.Request.Headers["Accept-Language"].ToString().Split(',')[0].ToLower();

        if (new[] { "ru", "az", "en" }.Contains(queryLang))
            return queryLang;

        if (new[] { "ru", "az", "en" }.Contains(headerLang))
            return headerLang;

        return defaultLang;
    }
}