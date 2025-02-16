namespace Books.Core.Dtos.Update;

public class UpdateOrderDto
{
    public List<UpdateOrderItemDto> OrderItems { get; set; } = [];
    public decimal? TotalPrice { get; set; }
    public int? StatusId { get; set; }
    public string? Address { get; set; }
}