using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class MessageEntity : BaseEntity
{
    [Required]
    public Guid RoomId { get; set; }

    [Required]
    public string SenderId { get; set; } = default!;

    [Required]
    public string Content { get; set; } = default!;
}
