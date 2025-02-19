namespace Books.Core.Dtos.Update;

public class UpdateBookDto
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? Year { get; set; }
    public string? Genre { get; set; }
    public decimal? Price { get; set; }
    public string? ISBN { get; set; }
    public string? Description { get; set; }
}