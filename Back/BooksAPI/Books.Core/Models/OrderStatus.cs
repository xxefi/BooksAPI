namespace Books.Core.Models;

public class OrderStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Order> Orders { get; set; } = [];
}