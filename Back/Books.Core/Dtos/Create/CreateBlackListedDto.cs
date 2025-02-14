namespace Books.Core.Dtos.Create;

public class CreateBlackListedDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public string? UserAgent { get; set; }
}