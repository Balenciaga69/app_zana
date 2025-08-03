using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class RoomParticipantConfiguration : IEntityTypeConfiguration<RoomParticipant>
{
    public void Configure(EntityTypeBuilder<RoomParticipant> builder)
    {
        builder.HasOne(e => e.Room).WithMany(e => e.Participants).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.User).WithMany(e => e.RoomParticipants).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder
            .HasIndex(e => new
            {
                e.RoomId,
                e.UserId,
                e.IsActive,
            })
            .IsUnique()
            .HasFilter("\"IsActive\" = true");
        builder.HasIndex(e => e.RoomId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.IsActive);

        // 確保同一房間內不能有重複的 DisplayName（在活躍狀態下）
        builder
            .HasIndex(e => new
            {
                e.RoomId,
                e.DisplayName,
                e.IsActive,
            })
            .IsUnique()
            .HasFilter("\"IsActive\" = true");
    }
}
