namespace Books.Core.Dtos.Auth;

public class UserCredentialsDto
{
    public Guid Id { get; set; }
    public string Password { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
}