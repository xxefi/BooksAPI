using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Auth;

public interface IUserActiveSessionsService
{
    Task<UserActiveSessions> AddUserDeviceTokenAsync(CreateUserDeviceTokenDto token);
    Task<UserActiveSessions> GetUserDeviceTokenAsync(Guid userId);
    Task<UserActiveSessions> UpdateUserDeviceTokenAsync(UpdateUserDeviceTokenDto token);
    Task<UserActiveSessions> DeleteUserDeviceTokenAsync(Guid tokenId);
    Task<bool> IsUserSessionActiveAsync(Guid userId);
}