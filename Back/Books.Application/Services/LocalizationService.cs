using System.Globalization;
using System.Resources;
using Books.Core.Abstractions.Services;

namespace Books.Application.Services;

public class LocalizationService : ILocalizationService
{
    private readonly string _defaultCulture = "en";
    private readonly Dictionary<string, ResourceManager> _resourceManagers = new()
    {
        { "en", new ResourceManager("Books.Application.Resources.ValidationMessages.en", typeof(LocalizationService).Assembly) },
        { "ru", new ResourceManager("Books.Application.Resources.ValidationMessages.ru", typeof(LocalizationService).Assembly) },
        { "az", new ResourceManager("Books.Application.Resources.ValidationMessages.az", typeof(LocalizationService).Assembly) }
    };
    public string GetLocalizedString(string key, string? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (!_resourceManagers.TryGetValue(culture, out var resourceManager))
            resourceManager = _resourceManagers[_defaultCulture];
        
        return resourceManager.GetString(key) ?? key;
    }
}