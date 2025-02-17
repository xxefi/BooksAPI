using System.Linq.Expressions;
using Auth.Application.Exceptions;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Entities;
using Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class UserActiveSessionsRepository : IUserActiveSessionsRepository
{
    private readonly AuthContext _context;

    public UserActiveSessionsRepository(AuthContext context)
        => _context = context;
    
    public async Task<UserActiveSessions> GetByIdAsync(Guid id)
        => await _context.UserActiveSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false)
        ?? throw new AuthException(ExceptionType.NotFound, "UserDeviceTokenNotFound"); 

    public async Task<IEnumerable<UserActiveSessions>> GetAllAsync()
    {
        var userDeviceTokens = await _context.UserActiveSessions
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
        
        return userDeviceTokens.Any() ? userDeviceTokens : throw new AuthException(ExceptionType.NotFound, "NoUserDeviceTokensFound");
    }

    public async Task AddAsync(UserActiveSessions userActiveSessions)
        => await _context.UserActiveSessions.AddAsync(userActiveSessions);

    public async Task UpdateAsync(IEnumerable<UserActiveSessions> userDeviceTokens)
    {
        foreach (var userDeviceToken in userDeviceTokens)
        {
            var updatedCount = await _context.UserActiveSessions
                .Where(u => u.Id == userDeviceToken.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.UserId, userDeviceToken.UserId)
                    .SetProperty(u => u.AccessToken, userDeviceToken.AccessToken)
                    .SetProperty(u => u.RefreshToken, userDeviceToken.RefreshToken)
                    .SetProperty(u => u.RefreshTokenExpiryTime, userDeviceToken.RefreshTokenExpiryTime)
                    .SetProperty(u => u.DeviceInfo, userDeviceToken.DeviceInfo));
            
            if (updatedCount == 0)  throw new AuthException(ExceptionType.NotFound, "UserDeviceTokenNotFound"); 
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await _context.UserActiveSessions
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();
        
        if (item == 0) throw new AuthException(ExceptionType.NotFound, "UserDeviceTokenNotFound");
    }

    public async Task<bool> AnyAsync(Expression<Func<UserActiveSessions, bool>> predicate)
        => await _context.UserActiveSessions.AnyAsync(predicate);

    public async Task<ICollection<UserActiveSessions>> FindAsync(Expression<Func<UserActiveSessions, bool>> predicate)
        => await _context.UserActiveSessions
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
}