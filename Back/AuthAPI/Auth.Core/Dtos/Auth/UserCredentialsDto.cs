namespace Auth.Core.Dtos.Auth;

public class UserCredentialsDto
{
    public Guid Id { get; set; }
    public string Password { get; set; } = string.Empty;
}