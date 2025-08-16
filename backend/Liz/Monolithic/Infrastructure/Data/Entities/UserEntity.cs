using System.ComponentModel.DataAnnotations;

namespace Monolithic.Infrastructure.Data.Entities;

public class UserEntity : BaseEntity
{
    [Required]
    public string DeviceFingerprint { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime LastActiveAt { get; set; }

    [Required]
    public string Nickname { get; set; } = default!;
}
