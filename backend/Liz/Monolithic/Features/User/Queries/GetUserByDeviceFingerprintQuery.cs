using MediatR;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 根據設備指紋查找用戶查詢
/// </summary>
public class GetUserByDeviceFingerprintQuery : IRequest<GetUserByDeviceFingerprintResult?>
{
    public string DeviceFingerprint { get; set; } = default!;

    public GetUserByDeviceFingerprintQuery(string deviceFingerprint)
    {
        DeviceFingerprint = deviceFingerprint;
    }
}

/// <summary>
/// 根據設備指紋查找用戶查詢結果
/// </summary>
public class GetUserByDeviceFingerprintResult
{
    public Guid UserId { get; set; }
    public string? Nickname { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastActiveAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
