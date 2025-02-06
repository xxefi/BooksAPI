using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(Guid id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task UpdateAsync(IEnumerable<Role> roles);
    Task DeleteAsync(Guid id);
    Task<ICollection<Review>> FindAsync(Expression<Func<Review, bool>> predicate);
}