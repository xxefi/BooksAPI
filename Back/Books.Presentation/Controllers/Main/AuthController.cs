using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Main;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto) =>
        Ok(await _authService.LoginAsync(loginDto));

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto) =>
        Ok(await _authService.RefreshTokenAsync(tokenDto));

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout([FromBody] TokenDto tokenDto) =>
        Ok(await _authService.LogoutAsync(tokenDto));
}