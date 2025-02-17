namespace Auth.Core.Entities;

public class BlackListed
{
    public int Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; } = string.Empty;
}