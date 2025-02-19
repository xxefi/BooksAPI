using Books.Core.Entities;
using Books.Core.Enums;

namespace Books.Core.Dtos.Read;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}