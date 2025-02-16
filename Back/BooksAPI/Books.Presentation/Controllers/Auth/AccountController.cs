using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto) =>
        Ok(await _accountService.ChangePasswordAsync(changePasswordDto));

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto) =>
        Ok(await _accountService.ResetPasswordAsync(resetPasswordDto));
}