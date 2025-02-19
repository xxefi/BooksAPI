using System.Globalization;
using System.Resources;
using Auth.Core.Abstractions.Services;

namespace Auth.Application.Services;

public class LocalizationService : ILocalizationService
{
    private readonly string _defaultCulture = "en";
    private readonly Dictionary<string, Dictionary<string, ResourceManager>> _resourceManagers = new()
    {
        {
            "Errors",
            new()
            {
                { "en", new ResourceManager("Auth.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) },
                { "ru", new ResourceManager("Auth.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) },
                { "az", new ResourceManager("Auth.Application.Resources.Errors.ErrorsMessages", typeof(LocalizationService).Assembly) }
            }
        },  
        {
            "Validation",
            new()
            {
                { "en", new ResourceManager("Auth.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) },
                { "ru", new ResourceManager("Auth.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) },
                { "az", new ResourceManager("Auth.Application.Resources.Validators.ValidationMessages", typeof(LocalizationService).Assembly) }
            }
        }
    };
    
    public string GetLocalizedString(string key, string? culture = null)
    {
        culture ??= CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (!_resourceManagers.Values.SelectMany(dict => dict.Keys).Contains(culture))
        {
            culture = _defaultCulture;
        }

        foreach (var resourceDict in _resourceManagers.Values)
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