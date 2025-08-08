using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class UserConnection : BaseEntity
{
    [Required]
    public string UserId { get; set; } = default!;
    [Required]
    public string ConnectionId { get; set; } = default!;
    public DateTime ConnectedAt { get; set; }
    public DateTime? DisconnectedAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
