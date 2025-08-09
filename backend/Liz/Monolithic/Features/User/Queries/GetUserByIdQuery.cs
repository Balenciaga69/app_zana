using MediatR;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 根據 ID 查找用戶查詢
/// </summary>
public class GetUserByIdQuery : IRequest<GetUserByIdResult?>
{
    public Guid UserId { get; set; }

    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}

/// <summary>
/// 根據 ID 查找用戶查詢結果
/// </summary>
public class GetUserByIdResult
{
    public Guid UserId { get; set; }
    public string? Nickname { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastActiveAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DeviceFingerprint { get; set; } = default!;
}
