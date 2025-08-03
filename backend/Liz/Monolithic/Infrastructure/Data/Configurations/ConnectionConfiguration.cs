using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.HasOne(e => e.User).WithMany(e => e.Connections).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Room).WithMany(e => e.Connections).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(e => e.ConnectionId).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.RoomId);
        builder.HasIndex(e => e.IsActive);
    }
}
