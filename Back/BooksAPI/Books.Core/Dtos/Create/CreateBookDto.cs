namespace Books.Core.Dtos.Create;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int StatusId { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}