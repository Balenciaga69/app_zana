using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Content).IsRequired().HasMaxLength(2000);
        builder.HasIndex(m => m.RoomId);
        builder.HasIndex(m => m.SenderId);
        builder.Property(m => m.CreatedAt).IsRequired();
    }
}
