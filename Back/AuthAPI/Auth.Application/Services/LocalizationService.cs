using Auth.Core.Abstractions.Services;

namespace Auth.Application.Services;

public class LocalizationService : ILocalizationService
{
    public string GetLocalizedString(string key, string? culture = null)
    {
        throw new NotImplementedException();
    }
}