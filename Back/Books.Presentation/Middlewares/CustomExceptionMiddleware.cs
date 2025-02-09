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
    private readonly IServiceProvider _serviceProvider;
    private readonly ILocalizationService _localizationService;

    public CustomExceptionMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILocalizationService localizationService)
    {
        _next = next;
        _serviceProvider = serviceProvider;
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
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var errorResponse = CreateErrorResponse(context, exception, language, statusCode);

        if (exception is BookException academyExceptions)
        {
            statusCode = GetStatusCodeForExceptionType(academyExceptions.ExceptionType);
            errorResponse.Code = statusCode;
            errorResponse.Message = _localizationService.GetLocalizedString(academyExceptions.Message, language);
            errorResponse.Ex = academyExceptions.ExceptionType.ToString();
        }

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
            Ticks = DateTime.UtcNow.Ticks
        };
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

    private string GetLanguageFromRequest(HttpContext context)
    {
        string defaultLang = "en";
        var queryLang = context.Request.Query["lang"].ToString().ToLower();
        var headerLang = context.Request.Headers["Accept-Language"].ToString().Split(',')[0].ToLower();

        if (!string.IsNullOrEmpty(queryLang) && (queryLang == "ru" || queryLang == "az" || queryLang == "en"))
            return queryLang;

        if (!string.IsNullOrEmpty(headerLang) && (headerLang == "ru" || headerLang == "az" || headerLang == "en"))
            return headerLang;

        return defaultLang;
    }
}
