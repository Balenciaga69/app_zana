using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomParticipant> RoomParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.LastActiveAt);
                entity.HasIndex(e => e.IsOnline);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasOne(e => e.CreatedBy).WithMany(e => e.CreatedRooms).HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.CreatedById);
                entity.HasIndex(e => e.LastActivityAt);
                entity.HasIndex(e => e.IsActive);
            });

            modelBuilder.Entity<RoomParticipant>(entity =>
            {
                entity.HasOne(e => e.Room).WithMany(e => e.Participants).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User).WithMany(e => e.RoomParticipants).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasIndex(e => new
                    {
                        e.RoomId,
                        e.UserId,
                        e.IsActive,
                    })
                    .IsUnique()
                    .HasFilter("\"IsActive\" = true");

                entity.HasIndex(e => e.RoomId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.IsActive);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(e => e.Room).WithMany(e => e.Messages).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Sender).WithMany(e => e.Messages).HasForeignKey(e => e.SenderId).OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.RoomId);
                entity.HasIndex(e => e.SentAt).IsDescending();
                entity.HasIndex(e => new { e.RoomId, e.SentAt }).IsDescending();
            });

            modelBuilder.Entity<Connection>(entity =>
            {
                entity.HasOne(e => e.User).WithMany(e => e.Connections).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Room).WithMany(e => e.Connections).HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.ConnectionId).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.RoomId);
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}
