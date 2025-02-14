namespace Books.Core.Dtos.Read;

public class BlackListedDto
{
    public int Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AddedAt { get; set; }
}