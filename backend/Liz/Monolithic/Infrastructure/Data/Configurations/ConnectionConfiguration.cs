using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        // 關聯設定
        builder.HasOne(e => e.User).WithMany(e => e.Connections).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Room).WithMany(e => e.Connections).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(e => e.DeviceFingerprint)
            .WithMany(e => e.Connections)
            .HasForeignKey(e => e.DeviceFingerprintId)
            .OnDelete(DeleteBehavior.SetNull);

        // 索引設定
        builder.HasIndex(e => e.ConnectionId).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.RoomId);
        builder.HasIndex(e => e.DeviceFingerprintId);
        builder.HasIndex(e => e.IsActive);

        // 屬性設定
        builder.Property(c => c.UserAgent).HasMaxLength(500);
        builder.Property(c => c.IpAddress).HasMaxLength(45);
    }
}
