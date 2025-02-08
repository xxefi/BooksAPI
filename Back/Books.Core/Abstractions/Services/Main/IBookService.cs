using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Main;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookByIdAsync(Guid id);
    Task<BookDto?> GetBookByTitleAsync(string title);
    Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
    Task<BookDto> UpdateBookAsync(Guid id, UpdateBookDto updateBookDto);
    Task<bool> DeleteBookAsync(Guid id);
    Task<bool> ExistsByTitleAsync(string title);
    Task<int> GetBooksCountAsync();
    Task<IEnumerable<BookDto>> GetBooksPageAsync(int pageNumber, int pageSize);
}