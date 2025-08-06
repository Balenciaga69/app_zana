using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Monolithic.Infrastructure.Data.Entities.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // 索引設定
        builder.HasIndex(e => e.LastActiveAt);
        builder.HasIndex(e => e.IsOnline);

        // 屬性設定
        builder.Property(u => u.IsOnline).HasDefaultValue(false);
    }
}
