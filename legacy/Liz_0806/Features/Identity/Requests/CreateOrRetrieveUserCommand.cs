using MediatR;
using Monolithic.Features.Identity.Models;

namespace Monolithic.Features.Identity.Requests;

/// <summary>
/// 建立或找回用戶的 Command
/// </summary>
public class CreateOrRetrieveUserCommand : IRequest<UserSession>
{
    public string? BrowserFingerprint { get; set; }
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string? DeviceType { get; set; }
    public string? OperatingSystem { get; set; }
    public string? Browser { get; set; }
    public string? BrowserVersion { get; set; }
    public string? Platform { get; set; }
}
