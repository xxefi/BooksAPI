using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Main;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync();
    Task<OrderItemDto?> GetOrderItemByIdAsync(Guid id);
    Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto createOrderItemDto);
    Task<OrderItemDto> UpdateOrderItemAsync(Guid id, UpdateOrderItemDto updateOrderItemDto);
    Task<bool> DeleteOrderItemAsync(Guid id);
    Task<int> GetOrderItemsCountAsync();
    Task<IEnumerable<OrderItemDto>> GetOrderItemsPageAsync(int pageNumber, int pageSize);
}