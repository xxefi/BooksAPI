using System.Linq.Expressions;
using Auth.Application.Exceptions;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Entities;
using Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AuthContext _context;

    public RoleRepository(AuthContext context)
        => _context = context;
    
    public async Task<Role> GetByIdAsync(Guid id)
        => await _context.Roles
               .Include(r => r.Users)
               .AsNoTracking()
               .FirstOrDefaultAsync(r => r.Id == id)
           ?? throw new AuthException(ExceptionType.NotFound, "RoleNotFound");

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await _context.Roles
            .Include(r => r.Users)
            .AsNoTracking()
            .ToListAsync();
        
        return roles.Any() ? roles : throw new AuthException(ExceptionType.NotFound, "NoRolesFound");
    }

    public async Task AddAsync(Role role)
        => await _context.Roles.AddAsync(role);

    public async Task UpdateAsync(IEnumerable<Role> roles)
    {
        foreach (var role in roles)
        {
            var updatedCount = await _context.Roles
                .Where(r => r.Id == role.Id)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(r => r.Name, role.Name));

            if (updatedCount == 0) throw new AuthException(ExceptionType.NotFound, "RoleNotFound");
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var deletedCount = await _context.Roles
            .Where(r => r.Id == id)
            .ExecuteDeleteAsync();

        if (deletedCount == 0) throw new AuthException(ExceptionType.NotFound, "RoleNotFound");
    }

    public async Task<int> CountAsync()
        => await _context.Roles.CountAsync();

    public async Task<bool> AnyAsync(Expression<Func<Role, bool>> predicate)
        => await _context.Roles.AnyAsync(predicate);

    public async Task<ICollection<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        => await _context.Roles
            .Include(r => r.Users)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
}