using Microsoft.AspNetCore.SignalR;
using Monolithic.Shared.Extensions;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    private readonly IAppLogger<CommunicationHub> _logger;

    public CommunicationHub(IAppLogger<CommunicationHub> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// 連線建立事件
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var connectionInfo = ExtractConnectionInfo();

        // 發送連線確認事件給客戶端
        await Clients.Caller.SendAsync("ConnectionEstablished", connectionInfo.ConnectionId, DateTime.UtcNow);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// 心跳檢查
    /// 用於維持連線活躍度
    /// </summary>
    public async Task Heartbeat()
    {
        var connectionId = Context.ConnectionId;

        await Clients.Caller.SendAsync("HeartbeatResponse", DateTime.UtcNow);
    }

    /// <summary>
    /// 基本連通性測試
    /// 用於驗證 SignalR 連線功能
    /// </summary>
    public async Task Ping()
    {
        var connectionId = Context.ConnectionId;
        var timestamp = DateTime.UtcNow;

        await Clients.Caller.SendAsync("Pong", timestamp);
    }

    /// <summary>
    /// 取得連線資訊
    /// 僅提供基本的連線狀態資訊
    /// </summary>
    public async Task GetConnectionInfo()
    {
        var connectionInfo = ExtractConnectionInfo();

        await Clients.Caller.SendAsync(
            "ConnectionInfo",
            new
            {
                connectionInfo.ConnectionId,
                ConnectedAt = DateTime.UtcNow,
                connectionInfo.IpAddress,
                connectionInfo.UserAgent,
            }
        );
    }

    /// <summary>
    /// 提取連線基本資訊
    /// </summary>
    private ConnectionInfo ExtractConnectionInfo()
    {
        HttpContext? httpContext = null;
        try
        {
            httpContext = Context.GetHttpContext();
        }
        catch
        {
            // GetHttpContext() 在 Mock 環境中可能會失敗，忽略例外
        }

        return new ConnectionInfo
        {
            ConnectionId = Context.ConnectionId,
            IpAddress = httpContext != null ? httpContext.GetIpAddress() : null,
            UserAgent = httpContext != null ? httpContext.GetUserAgent() : null,
        };
    }
}

/// <summary>
/// 連線資訊數據傳輸物件
/// </summary>
internal record ConnectionInfo
{
    public required string ConnectionId { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
}
