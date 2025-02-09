using System.Security.Claims;
using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;
using static BCrypt.Net.BCrypt;

namespace Books.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthService(IMapper mapper,IUserService userService, ITokenService tokenService)
    {
        _mapper = mapper;
        _userService = userService;
        _tokenService = tokenService;
    }
    public async Task<AccessInfoDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userService.GetUserCredentialsByEmailAsync(loginDto.Email);
        
        if (!Verify(loginDto.Password, user.Password))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailOrPassword");
        
        var accessToken = _tokenService.GenerateAccessToken(_mapper.Map<UserDto>(user));
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.Now.AddDays(7);
        
        
        var updatedUserDto = new UpdateUserDto
        {
            Id = user.Id,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
        };
        
        await _userService.UpdateUserAsync(updatedUserDto.Id, updatedUserDto);

        return new AccessInfoDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

    }

    public async Task<AccessInfoDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        var user = await _userService.GetUserCredentialsByEmailAsync(tokenDto.Email);
        if (user.RefreshToken != tokenDto.RefreshToken)
            throw new BookException(ExceptionType.InvalidRequest, "InvalidRefreshToken");
        
        if (user.RefreshTokenExpiryTime < DateTime.Now)
            throw new BookException(ExceptionType.InvalidRequest, "RefreshTokenExpired");
        
        var accessToken = _tokenService.GenerateAccessToken(_mapper.Map<UserDto>(user));
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.Now.AddDays(7);
        
        var updatedUserDto = new UpdateUserDto
        {
            Id = user.Id,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime
        };
        
        await _userService.UpdateUserAsync(updatedUserDto.Id, updatedUserDto);
        
        return new AccessInfoDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };

    }

    public async Task<string> LogoutAsync(TokenDto tokenDto)
    {
        var user = await _userService.GetUserCredentialsByEmailAsync(tokenDto.Email);
        
        if (user.RefreshToken != tokenDto.RefreshToken)
            throw new BookException(ExceptionType.InvalidRequest, "InvalidRefreshToken");
        
        var updatedUserDto = new UpdateUserDto
        {
            Id = user.Id,
            RefreshToken = null,
            RefreshTokenExpiryTime = DateTime.Now 
        };
        
        await _userService.UpdateUserAsync(updatedUserDto.Id, updatedUserDto);
        return updatedUserDto.Id.ToString();
    }
}