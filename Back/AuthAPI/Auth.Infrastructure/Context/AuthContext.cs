using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Context;

public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<BlackListed> BlackListeds { get; set; }
    public DbSet<UserActiveSessions> UserActiveSessions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthContext).Assembly);
}