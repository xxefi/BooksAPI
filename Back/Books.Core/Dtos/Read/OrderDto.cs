namespace Books.Core.Dtos.Read;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; } = null!;
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public OrderStatusDto Status { get; set; } = null!;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}