using System.Linq.Expressions;
using Auth.Core.Entities;

namespace Auth.Core.Abstractions.Repositories;

public interface IUserActiveSessionsRepository
{
    Task<UserActiveSessions> GetByIdAsync(Guid id);
    Task<IEnumerable<UserActiveSessions>> GetAllAsync();
    Task AddAsync(UserActiveSessions userActiveSessions);
    Task UpdateAsync(IEnumerable<UserActiveSessions> userDeviceTokens);
    Task DeleteAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<UserActiveSessions, bool>> predicate);
    Task<ICollection<UserActiveSessions>> FindAsync(Expression<Func<UserActiveSessions, bool>> predicate);
}