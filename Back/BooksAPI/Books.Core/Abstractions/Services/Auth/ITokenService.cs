using System.Security.Claims;
using Books.Core.Dtos.Read;

namespace Books.Core.Abstractions.Services.Auth;

public interface ITokenService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    string GenerateRandomPassword(int length = 12);
}