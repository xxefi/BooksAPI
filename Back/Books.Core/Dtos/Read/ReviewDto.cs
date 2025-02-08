namespace Books.Core.Dtos.Read;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public DateTime CreatedAt { get; set; }
}