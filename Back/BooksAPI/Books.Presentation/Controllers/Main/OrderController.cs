using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Main;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
        => _orderService = orderService;

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _orderService.GetAllOrdersAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _orderService.GetOrderByIdAsync(id));

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto) =>
        Ok(await _orderService.CreateOrderAsync(createOrderDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderDto updateOrderDto) =>
        Ok(await _orderService.UpdateOrderAsync(id, updateOrderDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _orderService.DeleteOrderAsync(id));

    [HttpGet("GetOrdersPage")]
    public async Task<IActionResult> GetOrdersPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _orderService.GetOrdersPageAsync(pageNumber, pageSize));

    [HttpGet("GetOrdersCount")]
    public async Task<IActionResult> GetOrdersCount() =>
        Ok(await _orderService.GetOrdersCountAsync());
}
