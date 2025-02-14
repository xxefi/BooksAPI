namespace Books.Core.Dtos.Update;

public class UpdateUserDeviceTokenDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string AccessToken { get; set; }
    public string DeviceInfo { get; set; }
}