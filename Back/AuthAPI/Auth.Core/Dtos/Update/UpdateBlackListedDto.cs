namespace Auth.Core.Dtos.Update;

public class UpdateBlackListedDto
{
    public int Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}