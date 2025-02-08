using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(IEnumerable<User> users);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<bool> AnyAsync(Expression<Func<User, bool>> predicate);
    Task<ICollection<User>> FindAsync(Expression<Func<User, bool>> predicate);
}