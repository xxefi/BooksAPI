namespace Auth.Core.Dtos.Update;

public class UpdateUserCredentialsDto
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
}