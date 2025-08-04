using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class User : BaseEntity
{
    [Required]
    public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;

    public bool IsOnline { get; set; } = false;

    // Navigation properties
    public ICollection<Room> CreatedRooms { get; set; } = new List<Room>();
    public ICollection<RoomParticipant> RoomParticipants { get; set; } = new List<RoomParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    public ICollection<DeviceFingerprint> DeviceFingerprints { get; set; } = new List<DeviceFingerprint>();
}
