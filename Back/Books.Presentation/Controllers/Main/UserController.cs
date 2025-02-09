using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Main;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
        => _userService = userService;

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _userService.GetAllUsersAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _userService.GetUserByIdAsync(id) ?? throw new BookException(ExceptionType.NotFound, "UserNotFound"));

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsername(string username) =>
        Ok(await _userService.GetUserByUsernameAsync(username) ?? throw new BookException(ExceptionType.NotFound, "UserNotFound"));

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email) =>
        Ok(await _userService.GetUserByEmailAsync(email) ?? throw new BookException(ExceptionType.NotFound, "UserNotFound"));

    [HttpPost("CreateUser")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto) =>
        Ok(await _userService.CreateUserAsync(createUserDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto updateUserDto) =>
        Ok(await _userService.UpdateUserAsync(id, updateUserDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _userService.DeleteUserAsync(id));

    [HttpGet("exists/email/{email}")]
    public async Task<IActionResult> ExistsByEmail(string email) =>
        Ok(await _userService.ExistsByEmailAsync(email));

    [HttpGet("{userId:guid}/role")]
    public async Task<IActionResult> GetUserRole(Guid userId) =>
        Ok(await _userService.GetUserRoleAsync(userId));

    [HttpGet("roles/{roleId:guid}/users")]
    public async Task<IActionResult> GetUsersByRole(Guid roleId) =>
        Ok(await _userService.GetUsersByRoleAsync(roleId));

    [HttpGet("GetTotalUsersCount")]
    public async Task<IActionResult> GetTotalUsersCount() =>
        Ok(await _userService.GetTotalUsersCountAsync());

    [HttpGet("GetUsersPage")]
    public async Task<IActionResult> GetUsersPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _userService.GetUsersPageAsync(pageNumber, pageSize));
}