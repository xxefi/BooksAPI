using System.Linq.Expressions;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Models;
using Books.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Books.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BooksContext _context;

    public UserRepository(BooksContext context)
        => _context = context;


    public async Task<User> GetByIdAsync(Guid id)
        => await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new BookException(ExceptionType.NotFound, "UserNotFound");

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .ToListAsync();

        return users.Any() ? users : throw new BookException(ExceptionType.NotFound, "NoUsersFound");
    }

    public async Task AddAsync(User user)
        => await _context.Users.AddAsync(user);

    public async Task UpdateAsync(IEnumerable<User> users)
    {
        foreach (var user in users)
        {
            var existingUser = await _context.Users
                .Where(b => b.Id == user.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Username, user.Username)
                    .SetProperty(u => u.FirstName, user.FirstName)
                    .SetProperty(u => u.LastName, user.LastName)
                    .SetProperty(u => u.Email, user.Email)
                    .SetProperty(u => u.Password, user.Password)
                    .SetProperty(u => u.RoleId, user.RoleId)
                    .SetProperty(u => u.RefreshToken, user.RefreshToken)
                    .SetProperty(u => u.RefreshTokenExpiryTime, user.RefreshTokenExpiryTime));
            
            if (existingUser == 0) throw new BookException(ExceptionType.NotFound, "UserNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var deletedCount = await _context.Users
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();
        
        if (deletedCount == 0) throw new BookException(ExceptionType.NotFound, "UserNotFound");
    }

    public async Task<int> CountAsync()
        => await _context.Users.CountAsync();

    public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
        => await _context.Users.AnyAsync(predicate);

    public async Task<ICollection<User>> FindAsync(Expression<Func<User, bool>> predicate)
        => await _context.Users
            .Include(u => u.Role)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
}