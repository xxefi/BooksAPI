using Account.Grpc;
using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Auth;
using Grpc.Core;

namespace Auth.Presentation.Services;

public class AccountServiceImpl : AccountService.AccountServiceBase
{
    private readonly IAccountService _accountService;

    public AccountServiceImpl(IAccountService accountService)
        => _accountService = accountService;
    
    public override async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
    {
        try
        {
            var changePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            var success = await _accountService.ChangePasswordAsync(changePasswordDto);
                
            return new ChangePasswordResponse { Success = success };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
    }
}