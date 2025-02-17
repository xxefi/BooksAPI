using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Books.Infrastructure.Configurations;

public class BlackListedConfiguration : IEntityTypeConfiguration<BlackListed>
{
    public void Configure(EntityTypeBuilder<BlackListed> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(b => b.AccessToken)
            .IsRequired();
        builder.Property(b => b.RefreshToken)
            .IsRequired();

        builder.Property(b => b.AddedAt);
        
        builder.HasIndex(b => b.RefreshToken).IsUnique();
    }
}