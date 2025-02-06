using System.Linq.Expressions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;

namespace Books.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BooksContext _context;

    public OrderRepository(BooksContext context)
        => _context = context;


    public async Task<Order> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(IEnumerable<Order> orders)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}