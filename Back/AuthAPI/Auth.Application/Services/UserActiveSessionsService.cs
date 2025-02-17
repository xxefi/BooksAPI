using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;

namespace Auth.Application.Services;

public class UserActiveSessionsService : IUserActiveSessionsService
{
    public async Task<UserActiveSessions> AddUserActiveSessionAsync(CreateUserActiveSessionDto token)
    {
        throw new NotImplementedException();
    }

    public async Task<UserActiveSessions> GetUserActiveSessionAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserActiveSessions> UpdateUserActiveSessionAsync(UpdateUserActiveSessionDto token)
    {
        throw new NotImplementedException();
    }

    public async Task<UserActiveSessions> DeleteUserActiveSessionAsync(Guid tokenId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserSessionActiveAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}