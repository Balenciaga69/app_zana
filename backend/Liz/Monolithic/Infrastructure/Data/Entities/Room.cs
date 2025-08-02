using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

public class Room : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Password { get; set; }

    [Range(1, 10)]
    public int MaxParticipants { get; set; } = 10;

    [Required]
    [ForeignKey("CreatedBy")]
    public Guid CreatedById { get; set; }

    [Required]
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public User CreatedBy { get; set; } = null!;
    public ICollection<RoomParticipant> Participants { get; set; } = new List<RoomParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
