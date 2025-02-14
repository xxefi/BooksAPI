using Books.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class BlackListedConfiguration : IEntityTypeConfiguration<BlackListed>
{
    public void Configure(EntityTypeBuilder<BlackListed> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(b => b.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.AddedAt);
        
        builder.HasIndex(b => b.Token).IsUnique();
    }
}