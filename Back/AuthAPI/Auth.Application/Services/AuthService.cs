using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Auth;

namespace Auth.Application.Services;

public class AuthService : IAuthService
{
    public async Task<AccessInfoDto> LoginAsync(LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    public async Task<AccessInfoDto> RefreshTokenAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public Guid GetUserIdFromAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}