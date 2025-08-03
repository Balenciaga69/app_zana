using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // 既有索引
        builder.HasIndex(e => e.LastActiveAt);
        builder.HasIndex(e => e.IsOnline);

        // 新增瀏覽器指紋索引（用於快速查找）
        builder.HasIndex(e => e.BrowserFingerprint).HasDatabaseName("IX_Users_BrowserFingerprint");

        // 屬性設定
        builder.Property(u => u.IsOnline).HasDefaultValue(false);

        // 瀏覽器指紋相關欄位限制
        builder.Property(u => u.BrowserFingerprint).HasMaxLength(255);

        builder.Property(u => u.UserAgent).HasMaxLength(500);

        builder.Property(u => u.IpAddress).HasMaxLength(45);

        builder.Property(u => u.DeviceType).HasMaxLength(50);

        builder.Property(u => u.OperatingSystem).HasMaxLength(100);

        builder.Property(u => u.Browser).HasMaxLength(100);

        builder.Property(u => u.BrowserVersion).HasMaxLength(50);

        builder.Property(u => u.Platform).HasMaxLength(100);
    }
}
