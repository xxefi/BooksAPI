using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories;

public interface IBlackListedRepository
{
    Task<BlackListed> GetByIdAsync(int id);
    Task<IEnumerable<BlackListed>> GetAllAsync();
    Task AddAsync(BlackListed blackListed);
    Task UpdateAsync(IEnumerable<BlackListed> blackLists);
    Task DeleteAsync(int id);
    Task<bool> AnyAsync(Expression<Func<BlackListed, bool>> predicate);
    Task<ICollection<BlackListed>> FindAsync(Expression<Func<BlackListed, bool>> predicate);
}