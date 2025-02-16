using System.Linq.Expressions;
using Books.Core.Models;

namespace Books.Core.Abstractions.Repositories.Main;

public interface IBookRepository
{
    Task<Book> GetByIdAsync(Guid id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task AddAsync(Book book);
    Task UpdateAsync(IEnumerable<Book> books);
    Task DeleteAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<Book, bool>> predicate);
    Task<ICollection<Book>> FindAsync(Expression<Func<Book, bool>> predicate);
}