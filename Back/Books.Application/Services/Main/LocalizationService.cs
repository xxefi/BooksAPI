using System.Globalization;
using System.Resources;
using Books.Core.Abstractions.Services.Main;

namespace Books.Application.Services.Main;

public class LocalizationService : ILocalizationService
{
    private readonly string _defaultCulture = "en";
    private readonly List<Dictionary<string, ResourceManager>> _resourceManagers = new()
    {
        new()
        {
            { "en", new ResourceManager("Books.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) },
            { "ru", new ResourceManager("Books.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) },
            { "az", new ResourceManager("Books.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) }
        },
        new()
        {
            { "en", new ResourceManager("Books.Application.Resources.OrderStatus.OrderStatusMessages", typeof(LocalizationService).Assembly) },
            { "ru", new ResourceManager("Books.Application.Resources.OrderStatus.OrderStatusMessages", typeof(LocalizationService).Assembly) },
            { "az", new ResourceManager("Books.Application.Resources.OrderStatus.OrderStatusMessages", typeof(LocalizationService).Assembly) }
        },
        new()
        {
            { "en", new ResourceManager("Books.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) },
            { "ru", new ResourceManager("Books.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) },
            { "az", new ResourceManager("Books.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) }
        }
    };
    public string GetLocalizedString(string key, string? culture = null)
    {
        culture ??= _defaultCulture;

        foreach (var resourceDict in _resourceManagers)
        {
            if (resourceDict.TryGetValue(culture, out var resourceManager))
            {
                var result = resourceManager.GetString(key);
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
        }
        return key;
    }
}