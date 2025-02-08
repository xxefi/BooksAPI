using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Books.Application.Services.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRoleService _roleService;

    public TokenService(IConfiguration configuration, IRoleService roleService)
    {
        _configuration = configuration;
        _roleService = roleService;
    }
    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var role = await _roleService.GetRoleByIdAsync(user.RoleId);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signingCredentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync()
        => Guid.NewGuid().ToString();

    public ClaimsPrincipal GetPrincipalFromTokenAsync(string token, bool validateLifetime = false)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["JWT:Issuer"],
            ValidAudience = _configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!)),
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new BookException(ExceptionType.InvalidToken, "InvalidTokenF");
            
            return principal;
        }
        catch (Exception ex)
        {
            throw new BookException(ExceptionType.InvalidToken, ex.Message);
        }
    }

    public async Task<string> GenerateRandomPasswordAsync(int length = 12)
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