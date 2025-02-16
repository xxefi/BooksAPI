using Books.Core.Dtos.Read;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Books.Presentation.Middlewares;

public class CustomSuccessResponseMiddleware
{
    private readonly RequestDelegate _next;

    public CustomSuccessResponseMiddleware(RequestDelegate next) 
        => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();

            if (IsJsonResponse(context))
            {
                var modifiedResponse = BuildResponse(context, responseText);
                await HandleSuccessResponse(context, originalBodyStream, modifiedResponse);
            }
            else
            {
                await WriteRawResponse(originalBodyStream, responseText);
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseDto<object>
            {
                Success = false,
                Code = 500,
                Message = ex.Message,
                RequestDate = DateTime.UtcNow,
                Ticks = DateTime.UtcNow.Ticks,
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var errorJson = JsonSerializer.Serialize(errorResponse, GetJsonSerializerOptions());
            await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(errorJson));
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private static async Task HandleSuccessResponse(HttpContext context, Stream originalBodyStream, string modifiedResponse)
    {
        context.Response.ContentType = "application/json";
        await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(modifiedResponse));
    }

    private static async Task WriteRawResponse(Stream originalBodyStream, string responseText)
    {
        await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(responseText));
    }

    private static bool IsJsonResponse(HttpContext context)
    {
        var contentType = context.Response.ContentType;
        return !string.IsNullOrEmpty(contentType) &&
               Regex.IsMatch(contentType, @"application\/json", RegexOptions.IgnoreCase);
    }

    private static string BuildResponse(HttpContext context, string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText)) responseText = "{}";

        var data = JsonSerializer.Deserialize<object>(responseText);
        var responseObject = new ResponseDto<object>
        {
            Data = data,
            Success = true,
            Code = context.Response.StatusCode,
            RequestDate = DateTime.UtcNow,
            Ticks = DateTime.UtcNow.Ticks,
        };

        return JsonSerializer.Serialize(responseObject, GetJsonSerializerOptions());
    }

    private static JsonSerializerOptions GetJsonSerializerOptions() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
