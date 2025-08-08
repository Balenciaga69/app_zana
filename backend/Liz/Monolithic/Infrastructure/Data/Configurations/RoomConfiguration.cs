using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.OwnerId).IsRequired();
        builder.Property(r => r.PasswordHash).HasMaxLength(256);
        builder.Property(r => r.InviteCode).IsRequired().HasMaxLength(32);
        builder.HasIndex(r => r.InviteCode).IsUnique();
        builder.HasIndex(r => r.IsActive);
    }
}
