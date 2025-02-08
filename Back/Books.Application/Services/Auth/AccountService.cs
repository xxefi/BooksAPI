using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using static BCrypt.Net.BCrypt;

namespace Books.Application.Services.Auth;

public class AccountService : IAccountService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AccountService(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var currentPassword = await _userService.GetUserPasswordHashAsync(changePasswordDto.UserId);

        if (!Verify(changePasswordDto.CurrentPassword, currentPassword))
            throw new BookException(ExceptionType.InvalidRequest, "OldPasswordIncorrect");
        
        var newPassword = HashPassword(changePasswordDto.NewPassword);
        await _userService.UpdateUserPasswordAsync(changePasswordDto.UserId, newPassword);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userService.GetUserByEmailAsync(resetPasswordDto.Email);

        var newPassword = await _tokenService.GenerateRandomPasswordAsync();
        var hashedPassword = HashPassword(newPassword);
        
       var updateUserDto = new UpdateUserDto { Password = hashedPassword };
       
       await _userService.UpdateUserAsync(user.Id, updateUserDto);

       return true;
    }
}