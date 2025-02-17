using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;

namespace Auth.Core.Abstractions.Services;

public interface IUserActiveSessionsService
{
    Task<UserActiveSessions> AddUserActiveSessionAsync(CreateUserActiveSessionDto token);
    Task<UserActiveSessions> GetUserActiveSessionAsync(Guid userId);
    Task<UserActiveSessions> UpdateUserActiveSessionAsync(UpdateUserActiveSessionDto token);
    Task<UserActiveSessions> DeleteUserActiveSessionAsync(Guid tokenId);
    Task<bool> IsUserSessionActiveAsync(Guid userId);
}