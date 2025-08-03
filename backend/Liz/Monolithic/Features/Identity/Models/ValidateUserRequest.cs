using System.ComponentModel.DataAnnotations;

namespace Monolithic.Features.Identity.Models;

public class ValidateUserRequest
{
    [Required]
    public Guid UserId { get; set; }

    [MaxLength(255)]
    public string? BrowserFingerprint { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }
}
