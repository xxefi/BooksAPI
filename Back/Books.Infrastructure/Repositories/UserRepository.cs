using System.Linq.Expressions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;

namespace Books.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BooksContext _context;

    public UserRepository(BooksContext context)
        => _context = context;


    public async Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(IEnumerable<User> users)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}