using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Read;

namespace Books.Core.Abstractions.Services.Auth;

public interface IAuthService
{
    Task<AccessInfoDto> LoginAsync(LoginDto loginDto);
    Task<AccessInfoDto> RefreshTokenAsync();
    Task<bool> LogoutAsync();
    Guid GetUserIdFromAccessToken(string accessToken);
}