using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace Books.Presentation.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
        => _next = next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        var defaultCulture = "en"; 

        if (context.Request.Headers.TryGetValue("Accept-Language", out var languages))
        {
            var culture = languages.ToString().Split(',').Select(lang => lang.Split(';')[0]).FirstOrDefault();
            
            if (culture is not null && !culture.StartsWith("en"))
            {
                culture = defaultCulture;
            }

            var cultureInfo = new CultureInfo(culture ?? defaultCulture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
        else
        {
            var cultureInfo = new CultureInfo(defaultCulture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        await _next(context);
    }
}