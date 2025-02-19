using Books.Core.Entities;
using Books.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class BooksConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(b => b.Author)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(b => b.Year)
            .IsRequired();
        builder.Property(b => b.Genre)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(b => b.CreatedAt)
            .IsRequired();
        builder.Property(b => b.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        builder.Property(b => b.ISBN)
            .HasMaxLength(17)
            .IsRequired();
        builder.Property(b => b.Description)
            .HasMaxLength(1000);

        builder.Property(b => b.BookStatus)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (BookStatus)Enum.Parse(typeof(BookStatus), v)
            );

        builder.HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

        builder.HasMany(b => b.OrderItems)
            .WithOne(o => o.Book)
            .HasForeignKey(o => o.BookId);
        
        
        builder.HasIndex(b => b.Author);
        builder.HasIndex(b => b.Genre);
    }
}