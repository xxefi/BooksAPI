using System.Linq.Expressions;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories.Main;
using Books.Core.Entities;
using Books.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories.Main;

public class BookRepository : IBookRepository
{
    private readonly BooksContext _context;

    public BookRepository(BooksContext context)
        => _context = context;
    
    public async Task<Book> GetByIdAsync(Guid id)
        => await _context.Books
               .Include(b => b.Reviews) 
               .AsNoTracking()
               .FirstOrDefaultAsync(b => b.Id == id)
           ?? throw new BookException(ExceptionType.NotFound, "BookNotFound");

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var books = await _context.Books
            .Include(b => b.Reviews)
            .AsNoTracking()
            .ToListAsync();
        
        return books.Any() ? books : throw new BookException(ExceptionType.NotFound, "NoBooksFound");
    }

    public async Task AddAsync(Book book)
        =>  await _context.Books.AddAsync(book);

    public async Task UpdateAsync(IEnumerable<Book> books)
    {
        foreach (var book in books)
        {
            var existingBook = await _context.Books
                .Where(b => b.Id == book.Id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(b => b.Title, book.Title)
                    .SetProperty(b => b.Author, book.Author)
                    .SetProperty(b => b.Year, book.Year)
                    .SetProperty(b => b.Genre, book.Genre));
            
            if (existingBook == 0) throw new BookException(ExceptionType.NotFound, "BookNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
        
        if (book == 0) throw new BookException(ExceptionType.NotFound, "BookNotFound");
    }

    public async Task<bool> AnyAsync(Expression<Func<Book, bool>> predicate)
        => await _context.Books.AnyAsync(predicate);

    public async Task<ICollection<Book>> FindAsync(Expression<Func<Book, bool>> predicate)
        => await _context.Books
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
}
