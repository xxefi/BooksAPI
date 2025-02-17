using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configurations;

public class BlackListedsConfiguration : IEntityTypeConfiguration<BlackListed>
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