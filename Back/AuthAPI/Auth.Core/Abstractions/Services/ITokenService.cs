using Auth.Core.Dtos.Read;

namespace Auth.Core.Abstractions.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    string GenerateRandomPassword(int length = 12);
}