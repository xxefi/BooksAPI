namespace Books.Core.Models;

public class UserActiveSessions
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;
}