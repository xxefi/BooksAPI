using System.Linq.Expressions;
using Books.Core.Entities;

namespace Books.Core.Abstractions.Repositories.Auth;

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