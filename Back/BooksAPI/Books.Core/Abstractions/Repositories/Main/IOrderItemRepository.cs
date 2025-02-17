using System.Linq.Expressions;
using Books.Core.Entities;

namespace Books.Core.Abstractions.Repositories.Main;

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