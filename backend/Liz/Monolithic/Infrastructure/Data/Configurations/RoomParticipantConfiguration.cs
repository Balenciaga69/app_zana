using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class RoomParticipantConfiguration : IEntityTypeConfiguration<RoomParticipantEntity>
{
    public void Configure(EntityTypeBuilder<RoomParticipantEntity> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.HasIndex(rp => new { rp.RoomId, rp.UserId });
        builder.Property(rp => rp.UserId).IsRequired();
    }
}
