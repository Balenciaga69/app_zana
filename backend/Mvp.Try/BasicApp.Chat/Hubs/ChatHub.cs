using BasicApp.Chat.Services;
using Microsoft.AspNetCore.SignalR;

namespace BasicApp.Chat.Hubs;

/// <summary>
/// 聊天室 Hub，處理 WebSocket 連線和 UserId 管理
/// </summary>
public class ChatHub : Hub, IChatHub
{
    private readonly IConnectionService _connectionService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IConnectionService connectionService, ILogger<ChatHub> logger)
    {
        _connectionService = connectionService;
        _logger = logger;
    }

    /// <summary>
    /// 連線建立時的處理
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var clientIpAddress = GetClientIpAddress();
        var userAgent = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString();

        _logger.LogInformation("New connection attempt: {ConnectionId} from {IpAddress}", connectionId, clientIpAddress);

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// 使用者主動註冊或重新連線（帶 UserId）
    /// </summary>
    /// <param name="existingUserId">現有的 UserId（可為空）</param>
    /// <returns>實際使用的 UserId</returns>
    public async Task<string> RegisterUser(string? existingUserId = null)
    {
        var connectionId = Context.ConnectionId;
        var clientIpAddress = GetClientIpAddress();
        var userAgent = Context.GetHttpContext()?.Request.Headers["User-Agent"].ToString();

        var userId = await _connectionService.AddConnectionAsync(connectionId, existingUserId, clientIpAddress, userAgent);

        _logger.LogInformation("User registered: {UserId} with connection {ConnectionId}", userId, connectionId);

        // 通知客戶端確認的 UserId
        await Clients.Caller.SendAsync("UserRegistered", userId);

        // 廣播使用者上線通知（可選）
        var onlineCount = await _connectionService.GetOnlineUserCountAsync();
        await Clients.All.SendAsync("OnlineCountChanged", onlineCount);

        return userId;
    }

    /// <summary>
    /// 發送訊息（暫時保持原有功能，但加入 UserId 驗證）
    /// </summary>
    public async Task SendMessage(string user, string message)
    {
        var connectionId = Context.ConnectionId;
        var userId = await _connectionService.GetUserIdByConnectionAsync(connectionId);

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized message attempt from connection {ConnectionId}", connectionId);
            await Clients.Caller.SendAsync("Error", "請先註冊使用者");
            return;
        }

        // 更新活動時間
        await _connectionService.UpdateLastActivityAsync(connectionId);

        _logger.LogInformation("User {UserId} ({User}) sent message: {Message}", userId, user, message);
        await Clients.All.SendAsync("ReceiveMessage", user, message, userId);
    }

    /// <summary>
    /// 取得連線 ID（保持向下相容）
    /// </summary>
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    /// <summary>
    /// 取得目前在線統計資訊
    /// </summary>
    public async Task<object> GetOnlineStats()
    {
        var userCount = await _connectionService.GetOnlineUserCountAsync();
        var connectionCount = await _connectionService.GetTotalConnectionCountAsync();

        return new
        {
            OnlineUsers = userCount,
            TotalConnections = connectionCount,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// 連線中斷時的處理
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var userId = await _connectionService.GetUserIdByConnectionAsync(connectionId);

        if (userId != null)
        {
            _logger.LogInformation("User {UserId} disconnected (Connection: {ConnectionId})", userId, connectionId);
        }

        await _connectionService.RemoveConnectionAsync(connectionId);

        // 廣播使用者數量更新
        var onlineCount = await _connectionService.GetOnlineUserCountAsync();
        await Clients.All.SendAsync("OnlineCountChanged", onlineCount);

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// 取得客戶端 IP 位址
    /// </summary>
    private string? GetClientIpAddress()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext == null) return null;

        // 檢查是否有代理伺服器標頭
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = httpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // 回退到直接連線 IP
        return httpContext.Connection.RemoteIpAddress?.ToString();
    }
}
