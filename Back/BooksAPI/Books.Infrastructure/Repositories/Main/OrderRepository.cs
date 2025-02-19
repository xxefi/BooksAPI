using System.Linq.Expressions;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Entities;
using Books.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories.Main;

public class OrderRepository : IOrderRepository
{
    private readonly BooksContext _context;

    public OrderRepository(BooksContext context)
        => _context = context;


    public async Task<Order> GetByIdAsync(Guid id)
        => await _context.Orders
               .Include(o => o.User)
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Book)
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.Id == id)
           ?? throw new BookException(ExceptionType.NotFound, "OrderNotFound");

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Book)
            .AsNoTracking()
            .ToListAsync();
        
        return orders.Any() ? orders : throw new BookException(ExceptionType.NotFound, "NoOrdersFound");
    }

    public async Task AddAsync(Order order)
        => await _context.Orders.AddAsync(order);

    public async Task UpdateAsync(IEnumerable<Order> orders)
    {
        foreach (var order in orders)
        {
            var updatedCount = await _context.Orders
                .Where(o => o.Id == order.Id)
                .ExecuteUpdateAsync(o => o
                    .SetProperty(o => o.UserId, order.UserId)
                    .SetProperty(o => o.TotalPrice, order.TotalPrice)
                    .SetProperty(o => o.StatusId, order.StatusId)
                    .SetProperty(o => o.Address, order.Address));

            if (updatedCount == 0) throw new BookException(ExceptionType.NotFound, "OrderNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();
        
        if (order == 0) throw new BookException(ExceptionType.NotFound, "OrderNotFound");
    }

    public async Task<int> CountAsync()
        => await _context.Orders.CountAsync();

    public async Task<ICollection<Order>> FindAsync(Expression<Func<Order, bool>> predicate)
        => await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Book)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
}