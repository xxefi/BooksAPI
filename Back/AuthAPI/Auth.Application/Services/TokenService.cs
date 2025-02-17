using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Read;

namespace Auth.Application.Services;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(UserDto user)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public string GenerateRandomPassword(int length = 12)
    {
        throw new NotImplementedException();
    }
}