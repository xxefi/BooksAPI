using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthContext).Assembly);
}