using Auth.Core.Dtos.Auth;

namespace Auth.Core.Abstractions.Services;

public interface IAccountService
{
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}