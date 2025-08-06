using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

public class RoomParticipant : BaseEntity
{
    [Required]
    [ForeignKey("Room")]
    public Guid RoomId { get; set; }

    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Required]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LeftAt { get; set; }

    [Required]
    [MaxLength(50)]
    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Room Room { get; set; } = null!;
    public User User { get; set; } = null!;
}
