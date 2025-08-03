using System.ComponentModel.DataAnnotations;

namespace Monolithic.Features.Identity.Models;

public class CreateUserRequest
{
    [MaxLength(255)]
    public string? BrowserFingerprint { get; set; }

    [Required]
    [MaxLength(500)]
    public string UserAgent { get; set; } = string.Empty;

    [Required]
    [MaxLength(45)]
    public string IpAddress { get; set; } = string.Empty;

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
}
