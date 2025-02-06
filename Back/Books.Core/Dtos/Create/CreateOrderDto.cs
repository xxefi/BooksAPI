namespace Books.Core.Dtos.Create;

public class CreateOrderDto
{
    public Guid UserId { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public int StatusId { get; set; }
    public string Address { get; set; } = string.Empty;
}