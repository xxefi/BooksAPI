using Books.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .HasMaxLength(50);
        builder.Property(u => u.FirstName)
            .HasMaxLength(100);
        builder.Property(u => u.LastName)
            .HasMaxLength(100);
        builder.Property(u => u.Email)
            .HasMaxLength(100);
        builder.Property(u => u.Password)
            .HasMaxLength(255);
        builder.Property(u => u.CreatedAt);
        
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}