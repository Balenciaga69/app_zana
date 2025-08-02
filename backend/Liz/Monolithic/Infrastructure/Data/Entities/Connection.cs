using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

public class Connection : BaseEntity
{
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ConnectionId { get; set; } = string.Empty;

    [ForeignKey("Room")]
    public Guid? RoomId { get; set; }

    [Required]
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DisconnectedAt { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public User User { get; set; } = null!;
    public Room? Room { get; set; }
}
