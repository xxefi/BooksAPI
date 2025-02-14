using Books.Core.Abstractions.Services.Auth;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books.Presentation.Controllers.Auth;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BlackListedController : ControllerBase
{
    private readonly IBlackListedService _blackListedService;

    public BlackListedController(IBlackListedService blackListedService)
        => _blackListedService = blackListedService;
    
    [HttpGet]
    public async Task<IActionResult> GetAllBlackListed() 
        => Ok(await _blackListedService.GetAllBlackListedAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlackListedById(int id) 
        => Ok(await _blackListedService.GetBlackListedByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> AddToBlackList([FromBody] CreateBlackListedDto createBlackListedDto) 
        => Ok(await _blackListedService.AddToBlackListAsync(createBlackListedDto));

    [HttpPut]
    public async Task<IActionResult> UpdateBlackList([FromBody] IEnumerable<UpdateBlackListedDto> updateBlackListedDto) 
        => Ok(await _blackListedService.UpdateBlackListAsync(updateBlackListedDto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFromBlackList(int id) 
        => Ok(await _blackListedService.DeleteFromBlackListAsync(id));

    [HttpGet("check/{token}")]
    public async Task<IActionResult> IsBlackListed(string token) 
        => Ok(await _blackListedService.IsBlackListedAsync(token));
}