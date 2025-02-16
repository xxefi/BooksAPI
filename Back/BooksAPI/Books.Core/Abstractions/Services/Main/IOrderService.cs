using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Main;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(Guid id);
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<OrderDto> UpdateOrderAsync(Guid id, UpdateOrderDto updateOrderDto);
    Task<bool> DeleteOrderAsync(Guid id);
    Task<int> GetOrdersCountAsync();
    Task<IEnumerable<OrderDto>> GetOrdersPageAsync(int pageNumber, int pageSize);
}