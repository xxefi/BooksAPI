namespace Books.Core.Dtos.Create;

public class CreateUserActiveSessionDto
{
    public Guid UserId { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string DeviceInfo { get; set; }
}