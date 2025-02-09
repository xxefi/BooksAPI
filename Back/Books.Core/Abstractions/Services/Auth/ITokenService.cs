using System.Security.Claims;
using Books.Core.Dtos.Read;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime = false);
    string GenerateRandomPassword(int length = 12);
}