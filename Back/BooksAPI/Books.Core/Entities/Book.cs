using Books.Core.Enums;

namespace Books.Core.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public List<Review> Reviews { get; set; } = null!;
    public List<OrderItem> OrderItems { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Price { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public int BookStatusId { get; set; }
    public BookStatus BookStatus { get; set; }
    public string Description { get; set; } = string.Empty;
}