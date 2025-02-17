using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Auth;

namespace Auth.Application.Services;

public class AccountService : IAccountService
{
    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }
}