using Auth.Core.Dtos.Auth;

namespace Auth.Core.Abstractions.Services;

public interface IAuthService
{
    Task<AccessInfoDto> LoginAsync(LoginDto loginDto);
    Task<AccessInfoDto> RefreshTokenAsync();
    Task<bool> LogoutAsync(string accessToken, string refreshToken);
    Guid GetUserIdFromAccessToken(string accessToken);
}