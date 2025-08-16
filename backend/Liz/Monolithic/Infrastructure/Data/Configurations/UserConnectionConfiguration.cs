using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnectionEntity>
{
    public void Configure(EntityTypeBuilder<UserConnectionEntity> builder)
    {
        builder.HasKey(uc => uc.Id);
        builder.Property(uc => uc.ConnectionId).IsRequired().HasMaxLength(128);
        builder.Property(uc => uc.UserId).IsRequired();
        builder.Property(uc => uc.IpAddress).HasMaxLength(45);
        builder.Property(uc => uc.UserAgent).HasMaxLength(500);
        builder.HasIndex(uc => uc.UserId);
        builder.HasIndex(uc => uc.ConnectionId).IsUnique();
        builder.HasIndex(uc => uc.DisconnectedAt);
    }
}
