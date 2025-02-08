using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories;

public interface IOrderItemRepository
{
    Task<OrderItem> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderItem>> GetAllAsync();
    Task AddAsync(OrderItem orderItem);
    Task UpdateAsync(IEnumerable<OrderItem> orderItems);
    Task DeleteAsync(Guid id);
    Task<int> CountAsync();
    Task<ICollection<OrderItem>> FindAsync(Expression<Func<OrderItem, bool>> predicate);
}