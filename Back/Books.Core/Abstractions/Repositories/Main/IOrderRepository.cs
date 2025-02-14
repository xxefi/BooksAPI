using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories.Main;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task UpdateAsync(IEnumerable<Order> orders);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<ICollection<Order>> FindAsync(Expression<Func<Order, bool>> predicate);
}