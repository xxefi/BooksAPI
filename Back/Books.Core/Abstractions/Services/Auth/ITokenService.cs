using System.Security.Claims;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Auth;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync();
    ClaimsPrincipal GetPrincipalFromTokenAsync(string token, bool validateLifetime = false);
    Task<string> GenerateRandomPasswordAsync(int length = 12);
}