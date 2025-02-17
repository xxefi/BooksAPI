using System.Linq.Expressions;
using Books.Core.Entities;

namespace Books.Core.Abstractions.Repositories.Main;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task UpdateAsync(IEnumerable<Role> roles);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<bool> AnyAsync(Expression<Func<Role, bool>> predicate);
    Task<ICollection<Role>> FindAsync(Expression<Func<Role, bool>> predicate);
}