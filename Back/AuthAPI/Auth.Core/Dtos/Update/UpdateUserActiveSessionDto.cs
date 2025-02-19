namespace Auth.Core.Dtos.Update;

public class UpdateUserActiveSessionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryDate { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;
}