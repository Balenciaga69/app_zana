using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class RoomParticipantConfiguration : IEntityTypeConfiguration<RoomParticipant>
{
    public void Configure(EntityTypeBuilder<RoomParticipant> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.HasIndex(rp => new { rp.RoomId, rp.UserId });
        builder.Property(rp => rp.UserId).IsRequired();
    }
}
