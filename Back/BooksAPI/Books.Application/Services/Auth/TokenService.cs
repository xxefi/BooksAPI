using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Read;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Books.Application.Services.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
        => _configuration = configuration;
    public string GenerateAccessToken(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.RoleName),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new("CreatedAt", user.CreatedAt.ToString("o"))
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
              ?? throw new BookException(ExceptionType.NotFound, "SecretKeyNotFound");
        
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
        => Guid.NewGuid().ToString();
    
    public string GenerateRandomPassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        StringBuilder password = new();
        using var rng = new RNGCryptoServiceProvider();
        
        byte[] buffer = new byte[sizeof(uint)];

        while (length-- > 0)
        {
            rng.GetBytes(buffer);
            uint num = BitConverter.ToUInt32(buffer, 0);
            password.Append(validChars[(int)(num % (uint)validChars.Length)]);
        }
        
        return password.ToString();
    }
}