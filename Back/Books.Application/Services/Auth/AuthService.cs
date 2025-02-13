using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Constants;
using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Http.HttpResults;
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
        if (string.IsNullOrEmpty(loginDto?.Email) || string.IsNullOrEmpty(loginDto?.Password))
            throw new BookException(ExceptionType.InvalidRequest, "EmailAndPasswordCannotBeEmpty");
        
        if (!Regex.IsMatch(loginDto.Email, ValidationConstants.EmailRegex))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailFormat");
        
        var user = await _userService.GetUserByEmailAsync(loginDto.Email);
        var userCredentials = await _userService.GetUserCredentialsByIdAsync(user.Id);
        
        if (!Verify(loginDto.Password, userCredentials.Password))
            throw new BookException(ExceptionType.InvalidRequest, "InvalidEmailOrPassword");
        
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        if (!string.IsNullOrEmpty(user.RefreshToken) && user.RefreshTokenExpiryTime > DateTime.UtcNow)
            throw new BookException(ExceptionType.InvalidRefreshToken, "RefreshTokenAlreadyExists");
        

        var updatedUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime,
        };
        
        await _userService.UpdateUserCredentialsAsync(updatedUser.UserId, updatedUser);

        return new AccessInfoDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

    }

    public async Task<AccessInfoDto> RefreshTokenAsync(TokenDto tokenDto)
    {
        var user = await _userService.GetUserCredentialsByIdAsync(tokenDto.Id);
        if (user.RefreshToken != tokenDto.RefreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");
        
        if (user.RefreshTokenExpiryTime < DateTime.Now)
            throw new BookException(ExceptionType.InvalidRefreshToken, "RefreshTokenExpired");
        
        var accessToken = _tokenService.GenerateAccessToken(_mapper.Map<UserDto>(user));
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        var updateUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = refreshTokenExpiryTime
        };
        
        await _userService.UpdateUserCredentialsAsync(updateUser.UserId, updateUser);
        
        return new AccessInfoDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };

    }

    public async Task<bool> LogoutAsync(TokenDto tokenDto)
    {
        var user = await _userService.GetUserCredentialsByIdAsync(tokenDto.Id);
        
        if (user.RefreshToken != tokenDto.RefreshToken)
            throw new BookException(ExceptionType.InvalidRefreshToken, "InvalidRefreshToken");
        
        var updatedUser = new UpdateUserCredentialsDto
        {
            UserId = user.Id,
            RefreshToken = null,
            RefreshTokenExpiryTime = DateTime.UtcNow 
        };
        
        await _userService.UpdateUserCredentialsAsync(updatedUser.UserId, updatedUser);
        return true;
    }
}