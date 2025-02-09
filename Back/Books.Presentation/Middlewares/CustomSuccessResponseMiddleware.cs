using Books.Core.Dtos.Read;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Books.Infrastructure.Context;

namespace Books.Presentation.Middlewares;

public class CustomSuccessResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public CustomSuccessResponseMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksContext>();
        try
        {
            await _next(context);

            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();

            var modifiedResponse = await BuildResponseAsync(context, responseText);

            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                await HandleSuccessResponse(context, originalBodyStream, responseBody, modifiedResponse);
            else
                await HandleErrorResponse(originalBodyStream, responseBody);
        }
        catch (Exception ex)
        {
            var responseObject = new ResponseDto<object>
            {
                Success = false,
                Code = 500,
                Message = ex.Message,
                RequestDate = DateTime.UtcNow,
                Ticks = DateTime.UtcNow.Ticks,
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errorResponse = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(errorResponse));
        }
    }

    private static async Task HandleSuccessResponse(
        HttpContext context,
        Stream originalBodyStream,
        Stream responseBody,
        string modifiedResponse)
    {
        if (context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
        {
            context.Response.ContentType = "application/json";
            await originalBodyStream.WriteAsync(Encoding.UTF8.GetBytes(modifiedResponse));
        }
        else
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
    private static async Task HandleErrorResponse(Stream originalBodyStream, Stream responseBody)
    {
        responseBody.Seek(0, SeekOrigin.Begin);
        await responseBody.CopyToAsync(originalBodyStream);
    }
    private static async Task<string> BuildResponseAsync(HttpContext context, string responseText)
    {
        if (string.IsNullOrWhiteSpace(responseText)) responseText = "{}";

        var data = await JsonSerializer.DeserializeAsync<object>(
            new MemoryStream(Encoding.UTF8.GetBytes(responseText)));

        var responseObject = new ResponseDto<object>
        {
            Data = data,
            Success = true,
            Code = context.Response.StatusCode,
            RequestDate = DateTime.UtcNow,
            Ticks = DateTime.UtcNow.Ticks,
        };

        return JsonSerializer.Serialize(responseObject, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}
