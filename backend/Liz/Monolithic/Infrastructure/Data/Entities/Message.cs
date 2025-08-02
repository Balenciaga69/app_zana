using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monolithic.Infrastructure.Data.Entities;

public enum MessageType
{
    Text,
    System,
}

public class Message : BaseEntity
{
    [Required]
    [ForeignKey("Room")]
    public Guid RoomId { get; set; }

    [Required]
    [ForeignKey("Sender")]
    public Guid SenderId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;

    public MessageType MessageType { get; set; } = MessageType.Text;

    [Required]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Room Room { get; set; } = null!;
    public User Sender { get; set; } = null!;
}
