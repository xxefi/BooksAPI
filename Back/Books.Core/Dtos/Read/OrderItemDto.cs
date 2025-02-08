namespace Books.Core.Dtos.Read;

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid BookId { get; set; }
    public BookDto Book { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}