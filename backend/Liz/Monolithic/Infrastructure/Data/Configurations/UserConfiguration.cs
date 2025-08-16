using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.DeviceFingerprint).IsRequired().HasMaxLength(128);
        builder.HasIndex(u => u.DeviceFingerprint).IsUnique();
        builder.Property(u => u.Nickname).IsRequired().HasMaxLength(32);
        builder.HasIndex(u => u.IsActive);
    }
}
