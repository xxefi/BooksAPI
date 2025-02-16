using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Http;
using static BCrypt.Net.BCrypt;

namespace Books.Application.Services.Auth;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IUserService userService, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public HttpContext httpContext => _httpContextAccessor.HttpContext;
    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var accessToken = httpContext.Request.Cookies["accessToken"];
        var userId = GetUserIdFromAccessToken(accessToken);
        var currentPassword = await _userService.GetUserPasswordHashAsync(userId);

        if (!Verify(changePasswordDto.CurrentPassword, currentPassword))
            throw new BookException(ExceptionType.InvalidRequest, "OldPasswordIncorrect");
        
        var newPassword = HashPassword(changePasswordDto.NewPassword);
        await _userService.UpdateUserPasswordAsync(userId, newPassword);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userService.GetUserByEmailAsync(resetPasswordDto.Email);

        var newPassword = _tokenService.GenerateRandomPassword();
        var hashedPassword = HashPassword(newPassword);
        
        var updateUserDto = new UpdateUserDto { Password = hashedPassword };
       
        await _userService.UpdateUserAsync(user.Id, updateUserDto);

        return true;
    }
    
    public Guid GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ReadToken(accessToken) as JwtSecurityToken
                        ?? throw new BookException(ExceptionType.InvalidToken, "InvalidAccessToken");
        
        var userIdClaim = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userIdClaim!);
        return userId;
    }
}