using Auth.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configurations;

public class UserActiveSessionsConfiguration : IEntityTypeConfiguration<UserActiveSessions>
{
    public void Configure(EntityTypeBuilder<UserActiveSessions> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.AccessToken)
            .IsRequired();

        builder.Property(x => x.RefreshToken);

        builder.Property(x => x.RefreshTokenExpiryTime);

        builder.Property(x => x.DeviceInfo)
            .IsRequired();

        builder.HasIndex(x => x.RefreshToken).IsUnique(false);
    }
}