using System.Linq.Expressions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;

namespace Books.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly BooksContext _context;

    public RoleRepository(BooksContext context)
        => _context = context;
    
    public async Task<Role> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(IEnumerable<Role> roles)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Review>> FindAsync(Expression<Func<Review, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}