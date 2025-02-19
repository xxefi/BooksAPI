using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) =>
        Ok(await _authService.LoginAsync(loginDto));

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout() =>
        Ok(await _authService.LogoutAsync());
}