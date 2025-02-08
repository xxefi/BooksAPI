namespace Books.Core.Dtos.Update;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }  
    public string? LastName { get; set; }  
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Guid? RoleId { get; set; }
    public string? RefreshToken { get; set; } 
    public DateTime? RefreshTokenExpiryTime { get; set; }  
}