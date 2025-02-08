using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
        => _orderItemService = orderItemService;

    [HttpGet("GetOrderItems")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _orderItemService.GetAllOrderItemsAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _orderItemService.GetOrderItemByIdAsync(id) ?? throw new BookException(ExceptionType.NotFound, "OrderItemNotFound"));

    [HttpPost("CreateOrderItem")]
    public async Task<IActionResult> Create([FromBody] CreateOrderItemDto createOrderItemDto) =>
        Ok(await _orderItemService.CreateOrderItemAsync(createOrderItemDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderItemDto updateOrderItemDto) =>
        Ok(await _orderItemService.UpdateOrderItemAsync(id, updateOrderItemDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _orderItemService.DeleteOrderItemAsync(id));

    [HttpGet("GetOrderItemsPage")]
    public async Task<IActionResult> GetOrderItemsPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _orderItemService.GetOrderItemsPageAsync(pageNumber, pageSize));

    [HttpGet("GetOrderItemsCount")]
    public async Task<IActionResult> GetOrderItemsCount() =>
        Ok(await _orderItemService.GetOrderItemsCountAsync());
}