namespace Books.Core.Dtos.Auth;

public class TokenDto
{
    public Guid Id { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}