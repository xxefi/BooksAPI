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
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
        => _bookService = bookService;
    
    [HttpGet("GetBooks")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _bookService.GetAllBooksAsync());

    [HttpGet("ID/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) =>
        Ok(await _bookService.GetBookByIdAsync(id));

    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetByTitle(string title) =>
        Ok(await _bookService.GetBookByTitleAsync(title));

    [HttpPost("CreateBook")]
    public async Task<IActionResult> Create([FromBody] CreateBookDto createBookDto) =>
        Ok(await _bookService.CreateBookAsync(createBookDto));

    [HttpPut("Update/ID/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookDto updateBookDto) =>
        Ok(await _bookService.UpdateBookAsync(id, updateBookDto));

    [HttpDelete("Delete/ID/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) =>
        Ok(await _bookService.DeleteBookAsync(id));

    [HttpGet("exists/title/{title}")]
    public async Task<IActionResult> ExistsByTitle(string title) =>
        Ok(await _bookService.ExistsByTitleAsync(title));

    [HttpGet("GetBooksPage")]
    public async Task<IActionResult> GetBooksPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10) =>
        Ok(await _bookService.GetBooksPageAsync(pageNumber, pageSize));

    [HttpGet("GetBooksCount")]
    public async Task<IActionResult> GetBooksCount() =>
        Ok(await _bookService.GetBooksCountAsync());
}
