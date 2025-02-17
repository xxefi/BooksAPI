using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Content)
            .HasMaxLength(500);
        builder.Property(r => r.Rating)
            .HasDefaultValue(0) 
            .HasColumnType("decimal(2,1)");
        builder.Property(r => r.CreatedAt);
        
        builder.HasOne(r => r.Book)
            .WithMany(b => b.Reviews) 
            .HasForeignKey(r => r.BookId) 
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews) 
            .HasForeignKey(r => r.UserId) 
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.HasIndex(r => r.BookId);
        builder.HasIndex(r => r.UserId);
    }
}