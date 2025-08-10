// Communication 微服務 - SignalR Hub
// 職責：專注於即時通訊連線管理、訊息傳輸和廣播
// 邊界：不處理業務邏輯，未來將獨立為微服務
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.User.Services;
using Monolithic.Shared.Extensions;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Communication;

public class CommunicationHub : Hub
{
    private readonly IAppLogger<CommunicationHub> _logger;
    private readonly IUserCommunicationService _userCommunicationService;

    public CommunicationHub(
        IAppLogger<CommunicationHub> logger,
        IUserCommunicationService userCommunicationService
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userCommunicationService =
            userCommunicationService ?? throw new ArgumentNullException(nameof(userCommunicationService));
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
    /// 註冊/重新連線用戶
    /// Hub 只負責驗證與轉發，業務邏輯委派給 Service
    /// </summary>
    public async Task RegisterUser(string? existingUserId, string deviceFingerprint)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInfo(
            "[Communication] 用戶註冊請求",
            new
            {
                ExistingUserId = existingUserId,
                DeviceFingerprint = deviceFingerprint,
                ConnectionId = connectionId,
            },
            connectionId
        );

        try
        {
            // 基本驗證
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "設備指紋不能為空");
                return;
            }

            // 委派給 Service 處理業務邏輯
            await _userCommunicationService.RegisterUserAsync(
                existingUserId,
                deviceFingerprint,
                connectionId
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"[Communication] 用戶註冊失敗: {ex.Message}", ex, connectionId);
            await Clients.Caller.SendAsync("Error", $"註冊失敗: {ex.Message}");
        }
    }

    /// <summary>
    /// 即時更新暱稱
    /// Hub 只負責驗證與轉發，業務邏輯委派給 Service
    /// </summary>
    public async Task UpdateNickname(string newNickname)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInfo(
            "[Communication] 暱稱更新請求",
            new { NewNickname = newNickname, ConnectionId = connectionId },
            connectionId
        );

        try
        {
            // 基本驗證
            if (string.IsNullOrEmpty(newNickname))
            {
                await Clients.Caller.SendAsync("Error", "暱稱不能為空");
                return;
            }

            // 從 HttpContext 取得 deviceFingerprint（只信任伺服器端）
            var deviceFingerprint = Context.GetHttpContext()?.GetDeviceFingerprint();
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "無法驗證裝置指紋");
                return;
            }

            // 委派給 Service 處理業務邏輯
            await _userCommunicationService.UpdateNicknameAsync(newNickname, deviceFingerprint);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[Communication] 暱稱更新失敗: {ex.Message}", ex, connectionId);
            await Clients.Caller.SendAsync("Error", $"暱稱更新失敗: {ex.Message}");
        }
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

        try
        {
            // 委派給 Service 處理用戶斷線邏輯
            await _userCommunicationService.HandleUserDisconnectedAsync(connectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError($"[Communication] 處理用戶斷線失敗: {ex.Message}", ex, connectionId);
        }

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

        _logger.LogInfo(
            "[Communication] Ping 測試",
            new { ConnectionId = connectionId, Timestamp = timestamp },
            connectionId
        );

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
            IpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString(),
            UserAgent = httpContext?.Request?.Headers?.UserAgent.ToString(),
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
