namespace Monolithic.Features.Identity.Models;

public class UserSession
{
    public Guid UserId { get; set; }
    public DateTime LastActiveAt { get; set; }
    public bool IsOnline { get; set; }
    public string? BrowserFingerprint { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceType { get; set; }
    public string? OperatingSystem { get; set; }
    public string? Browser { get; set; }
    public string? BrowserVersion { get; set; }
    public string? Platform { get; set; }
    public DateTime CreatedAt { get; set; }
}
