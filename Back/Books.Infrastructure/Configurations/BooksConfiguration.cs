using Books.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(b => b.Year)
            .IsRequired();
        builder.Property(b => b.Genre)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

        builder.HasIndex(b => b.Author);
        builder.HasIndex(b => b.Genre);

    }
}