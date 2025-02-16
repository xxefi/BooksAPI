namespace Books.Core.Dtos.Create;

public class CreateReviewDto
{
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
}