using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

public class Connection : BaseEntity
{
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }

    /// <summary>
    /// 關聯的設備指紋ID（可選，因為可能是未知設備）
    /// </summary>
    [ForeignKey("DeviceFingerprint")]
    public Guid? DeviceFingerprintId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ConnectionId { get; set; } = string.Empty;

    [ForeignKey("Room")]
    public Guid? RoomId { get; set; }

    [Required]
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DisconnectedAt { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 連線時的 IP 位址快照
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// 連線時的 UserAgent 快照（用於日誌與分析）
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public DeviceFingerprint? DeviceFingerprint { get; set; }
    public Room? Room { get; set; }
}
