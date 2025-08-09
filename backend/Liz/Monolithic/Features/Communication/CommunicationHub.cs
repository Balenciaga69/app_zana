// Communication 微服務 - SignalR Hub
// 職責：專注於即時通訊連線管理、訊息傳輸和廣播
// 邊界：不處理業務邏輯，未來將獨立為微服務
using Microsoft.AspNetCore.SignalR;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Communication;

public class CommunicationHub : Hub
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

        _logger.LogInfo(
            "[Communication] 新連線建立",
            new
            {
                ConnectionId = connectionInfo.ConnectionId,
                UserAgent = connectionInfo.UserAgent,
                IpAddress = connectionInfo.IpAddress,
            },
            connectionInfo.ConnectionId
        );

        // 發送連線確認事件給客戶端
        await Clients.Caller.SendAsync("ConnectionEstablished", connectionInfo.ConnectionId, DateTime.UtcNow);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// 連線中斷事件
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInfo(
            "[Communication] 連線中斷",
            new
            {
                ConnectionId = connectionId,
                Exception = exception?.Message,
                DisconnectedAt = DateTime.UtcNow,
            },
            connectionId
        );

        // TODO: @Balenciaga69 未來透過 RabbitMQ 通知其他微服務
        // - 通知 User 微服務更新連線狀態
        // - 通知 Room 微服務處理離開房間

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 心跳檢查
    /// 用於維持連線活躍度
    /// </summary>
    public async Task Heartbeat()
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInfo("[Communication] 心跳檢查", new { ConnectionId = connectionId }, connectionId);

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

        _logger.LogInfo("[Communication] Ping 測試", new { ConnectionId = connectionId, Timestamp = timestamp }, connectionId);

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
        var httpContext = Context.GetHttpContext();

        return new ConnectionInfo
        {
            ConnectionId = Context.ConnectionId,
            IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
            UserAgent = httpContext?.Request.Headers.UserAgent.ToString(),
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
