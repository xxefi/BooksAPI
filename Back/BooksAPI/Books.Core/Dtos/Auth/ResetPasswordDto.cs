namespace Books.Core.Dtos.Auth;

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string ResetToken { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}