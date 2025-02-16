namespace Books.Core.Dtos.Create;

public class CreateOrderItemDto
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}