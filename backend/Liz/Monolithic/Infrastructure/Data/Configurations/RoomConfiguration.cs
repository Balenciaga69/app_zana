using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasOne(e => e.CreatedBy).WithMany(e => e.CreatedRooms).HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(e => e.CreatedById);
        builder.HasIndex(e => e.LastActivityAt);
        builder.HasIndex(e => e.IsActive);
    }
}
