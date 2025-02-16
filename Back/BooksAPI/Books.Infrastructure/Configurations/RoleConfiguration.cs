using Books.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .IsRequired() 
            .HasMaxLength(100);
        builder.Property(r => r.CreatedAt);
        
        builder.HasMany(r => r.Users) 
            .WithOne(u => u.Role) 
            .HasForeignKey(u => u.RoleId) 
            .OnDelete(DeleteBehavior.Restrict); 
        
        builder.HasIndex(r => r.Name).IsUnique();
    }
}