using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(e => e.Room).WithMany(e => e.Messages).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Sender).WithMany(e => e.Messages).HasForeignKey(e => e.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(e => e.RoomId);
        builder.HasIndex(e => e.SentAt).IsDescending();
        builder.HasIndex(e => new { e.RoomId, e.SentAt }).IsDescending();
    }
}
