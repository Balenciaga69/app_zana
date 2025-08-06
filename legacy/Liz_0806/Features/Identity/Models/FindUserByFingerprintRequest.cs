using System.ComponentModel.DataAnnotations;

namespace Monolithic.Features.Identity.Models;

public class FindUserByFingerprintRequest
{
    [Required]
    [MaxLength(255)]
    public string BrowserFingerprint { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }
}
