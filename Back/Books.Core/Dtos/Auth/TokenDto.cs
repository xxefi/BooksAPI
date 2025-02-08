namespace Books.Core.Dtos.Auth;

public class TokenDto
{
    public string Email { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}