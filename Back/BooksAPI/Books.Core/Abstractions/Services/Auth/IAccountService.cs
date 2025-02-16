using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Read;

namespace Books.Core.Abstractions.Services.Auth;

public interface IAccountService
{
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}