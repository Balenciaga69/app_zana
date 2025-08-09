using MediatR;

namespace Monolithic.Features.User.Queries;

/// <summary>
/// 查詢用戶連線狀態查詢
/// </summary>
public class GetUserConnectionsQuery : IRequest<GetUserConnectionsResult>
{
    public Guid UserId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;

    public GetUserConnectionsQuery(Guid userId, int skip = 0, int take = 20)
    {
        UserId = userId;
        Skip = skip;
        Take = take;
    }
}

/// <summary>
/// 查詢用戶連線狀態查詢結果
/// </summary>
public class GetUserConnectionsResult
{
    public IEnumerable<UserConnectionInfo> Connections { get; set; } = new List<UserConnectionInfo>();
    public int TotalCount { get; set; }
}

/// <summary>
/// 用戶連線資訊
/// </summary>
public class UserConnectionInfo
{
    public Guid Id { get; set; }
    public string ConnectionId { get; set; } = default!;
    public DateTime ConnectedAt { get; set; }
    public DateTime? DisconnectedAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
