using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class RoomParticipant : BaseEntity
{
    [Required]
    public Guid RoomId { get; set; }
    [Required]
    public string UserId { get; set; } = default!;
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
}
