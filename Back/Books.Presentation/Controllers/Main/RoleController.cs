using Books.Application.Exceptions;
using Books.Core.Abstractions.Services.Main;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Main;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
        => _roleService = roleService;

    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _roleService.GetAllRolesAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _roleService.GetRoleByIdAsync(id) ?? throw new BookException(ExceptionType.NotFound, "RoleNotFound"));

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name) =>
        Ok(await _roleService.GetRoleByNameAsync(name) ?? throw new BookException(ExceptionType.NotFound, "RoleNotFound"));

    [HttpPost("CreateRole")]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto roleDto) =>
        Ok(await _roleService.CreateRoleAsync(roleDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto roleDto) =>
        Ok(await _roleService.UpdateRoleAsync(id, roleDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _roleService.DeleteRoleAsync(id));

    [HttpGet("users/{roleId:guid}")]
    public async Task<IActionResult> GetUsersByRoleId(Guid roleId) =>
        Ok(await _roleService.GetUsersByRoleIdAsync(roleId));

    [HttpGet("rolesCount")]
    public async Task<IActionResult> GetTotalRolesCount() =>
        Ok(await _roleService.GetTotalRolesCountAsync());

    [HttpGet("{roleId:guid}/usersCount")]
    public async Task<IActionResult> GetUsersCountByRoleId(Guid roleId) =>
        Ok(await _roleService.GetUsersCountByRoleAsync(roleId));

    [HttpGet("GetRolesPage")]
    public async Task<IActionResult> GetRolesPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _roleService.GetRolesPageAsync(pageNumber, pageSize));
}