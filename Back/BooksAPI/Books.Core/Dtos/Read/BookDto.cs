using Books.Core.Enums;

namespace Books.Core.Dtos.Read;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public List<ReviewDto> Reviews { get; set; } = [];
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public decimal Price { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BookStatusId { get; set; }
    public string BookStatus { get; set; }
}