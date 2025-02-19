using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Auth;
using Auth.Grpc;
using Grpc.Core;

namespace Auth.Presentation.Services;

public class AuthServiceImpl : AuthService.AuthServiceBase
{
    private readonly IAuthService _authService;

    public AuthServiceImpl(IAuthService authService) => _authService = authService;

    public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
    {
        var loginDto = new LoginDto
        {
            Email = request.Email,
            Password = request.Password
        };

        var token = await _authService.LoginAsync(loginDto);

        return new LoginResponse
        {
           AccessToken = token.AccessToken,
           RefreshToken = token.RefreshToken
        };
    }
    
    public override async Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
    {
        var success = await _authService.LogoutAsync(request.AccessToken, request.RefreshToken);

        return new LogoutResponse
        {
            Success = success
        };
    }

}