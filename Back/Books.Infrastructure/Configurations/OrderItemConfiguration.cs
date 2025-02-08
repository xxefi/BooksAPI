using Books.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id); 

        builder.Property(oi => oi.Quantity)
            .IsRequired() 
            .HasDefaultValue(1); 

        builder.Property(oi => oi.Price)
            .HasPrecision(18, 2) 
            .IsRequired();

        builder.HasOne(oi => oi.Order) 
            .WithMany(o => o.OrderItems) 
            .HasForeignKey(oi => oi.OrderId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.Book) 
            .WithMany(b => b.OrderItems) 
            .HasForeignKey(oi => oi.BookId) 
            .OnDelete(DeleteBehavior.Restrict);
    }
}