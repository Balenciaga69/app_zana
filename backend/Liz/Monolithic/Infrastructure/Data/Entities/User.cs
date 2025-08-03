using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class User : BaseEntity
{
    [Required]
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

    public bool IsOnline { get; set; } = false;

    // 瀏覽器指紋相關欄位
    [MaxLength(255)]
    public string? BrowserFingerprint { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    [MaxLength(50)]
    public string? DeviceType { get; set; }

    [MaxLength(100)]
    public string? OperatingSystem { get; set; }

    [MaxLength(100)]
    public string? Browser { get; set; }

    [MaxLength(50)]
    public string? BrowserVersion { get; set; }

    [MaxLength(100)]
    public string? Platform { get; set; }

    // Navigation properties
    public ICollection<Room> CreatedRooms { get; set; } = new List<Room>();
    public ICollection<RoomParticipant> RoomParticipants { get; set; } = new List<RoomParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
}
