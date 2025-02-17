using System.Linq.Expressions;
using Books.Core.Entities;

namespace Books.Core.Abstractions.Repositories.Main;

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