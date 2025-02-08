namespace Books.Core.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public OrderStatus Status { get; set; } = null!;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}