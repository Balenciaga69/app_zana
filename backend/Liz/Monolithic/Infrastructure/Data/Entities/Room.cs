using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class Room : BaseEntity
{
    [Required]
    public string Name { get; set; } = default!;
    [Required]
    public string OwnerId { get; set; } = default!;
    public string? PasswordHash { get; set; }
    public int MaxParticipants { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastActiveAt { get; set; }
    [Required]
    public string InviteCode { get; set; } = default!;
    public DateTime? DestroyedAt { get; set; }
}
