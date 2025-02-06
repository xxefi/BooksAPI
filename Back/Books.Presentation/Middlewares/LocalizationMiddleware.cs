using System.Globalization;

namespace Books.Presentation.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
        =>  _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureQuery = context.Request.Headers["Accept-Language"].ToString();
        if (!string.IsNullOrWhiteSpace(cultureQuery))
        {
            var culture = cultureQuery.Split(',').FirstOrDefault();
            if (culture != null)
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }
        }
        await _next(context);
    }
}