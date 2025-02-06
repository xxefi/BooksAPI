using System.Linq.Expressions;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly BooksContext _context;

    public OrderItemRepository(BooksContext context)
        => _context = context;


    public async Task<OrderItem> GetByIdAsync(Guid id)
    {
        return await _context.OrderItems
                   .Include(oi => oi.Order)
                   .Include(oi => oi.Book)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(oi => oi.Id == id)
               ?? throw new BookException(ExceptionType.NotFound, "OrderItemNotFound");
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync()
    {
        var orderItems = await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Book)
            .AsNoTracking()
            .ToListAsync();
        
        return orderItems.Any() ? orderItems : throw new BookException(ExceptionType.NotFound, "NoOrderItemsFound");
    }

    public async Task AddAsync(OrderItem orderItem)
        => await _context.OrderItems.AddAsync(orderItem);

    public async Task UpdateAsync(IEnumerable<OrderItem> orderItems)
    {
        foreach (var orderItem in orderItems)
        {
            var existingOrderItem = await _context.OrderItems
                .Where(oi => oi.Id == orderItem.Id)
                .ExecuteUpdateAsync(oi => oi
                    .SetProperty(o => o.Quantity, orderItem.Quantity)
                    .SetProperty(o => o.Price, orderItem.Price)
                    .SetProperty(o => o.OrderId, orderItem.OrderId)
                    .SetProperty(o => o.BookId, orderItem.BookId)); 

            if (existingOrderItem == 0) throw new BookException(ExceptionType.NotFound, "OrderItemNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var orderItem = await _context.OrderItems
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
        
        if (orderItem == 0) throw new BookException(ExceptionType.NotFound, "OrderItemNotFound");
    }

    public async Task<ICollection<OrderItem>> FindAsync(Expression<Func<OrderItem, bool>> predicate)
    {
        var orderItem = await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Book)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
        
        return orderItem.Any() ? orderItem : Array.Empty<OrderItem>();
    }
}