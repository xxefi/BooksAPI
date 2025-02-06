namespace Books.Core.Dtos.Create;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty; 
    public string LastName { get; set; } = string.Empty;  
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}