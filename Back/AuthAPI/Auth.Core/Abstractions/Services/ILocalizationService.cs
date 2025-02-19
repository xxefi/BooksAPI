﻿namespace Auth.Core.Abstractions.Services;

public interface ILocalizationService
{
    string GetLocalizedString(string key, string? culture = null);
}