using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Auth;

public interface IUserActiveSessionsService
{
    Task<UserActiveSessions> AddUserActiveSessionAsync(CreateUserActiveSessionDto token);
    Task<UserActiveSessions> GetUserActiveSessionAsync(Guid userId);
    Task<UserActiveSessions> UpdateUserActiveSessionAsync(UpdateUserActiveSessionDto token);
    Task<UserActiveSessions> DeleteUserActiveSessionAsync(Guid tokenId);
    Task<bool> IsUserSessionActiveAsync(Guid userId);
}