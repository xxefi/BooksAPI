namespace Books.Core.Dtos.Create;

public class CreateBlackListedDto
{
    public string Token { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceInfo { get; set; }
    public string? UserAgent { get; set; }
}