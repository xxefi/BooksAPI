namespace Books.Core.Entities;

public class Review
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}