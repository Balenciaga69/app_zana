using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Infrastructure.Data.Configurations;

public class DeviceFingerprintConfiguration : IEntityTypeConfiguration<DeviceFingerprint>
{
    public void Configure(EntityTypeBuilder<DeviceFingerprint> builder)
    {
        // 索引設定
        builder.HasIndex(e => e.BrowserFingerprint).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.LastActiveAt);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => new { e.UserId, e.IsActive });

        // 屬性設定
        builder.Property(e => e.BrowserFingerprint).IsRequired().HasMaxLength(255);
        builder.Property(e => e.DeviceName).HasMaxLength(100);
        builder.Property(e => e.DeviceType).HasMaxLength(50);
        builder.Property(e => e.OperatingSystem).HasMaxLength(100);
        builder.Property(e => e.Browser).HasMaxLength(100);
        builder.Property(e => e.BrowserVersion).HasMaxLength(50);
        builder.Property(e => e.Platform).HasMaxLength(100);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.IsTrusted).HasDefaultValue(false);

        // 關聯設定
        builder.HasOne(e => e.User).WithMany(e => e.DeviceFingerprints).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
