using BasicApp.Chat.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicApp.Chat.Controllers;

/// <summary>
/// 連線監控 API 控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConnectionController : ControllerBase
{
    private readonly IConnectionService _connectionService;
    private readonly ILogger<ConnectionController> _logger;

    public ConnectionController(IConnectionService connectionService, ILogger<ConnectionController> logger)
    {
        _connectionService = connectionService;
        _logger = logger;
    }

    /// <summary>
    /// 取得線上統計資訊
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var userCount = await _connectionService.GetOnlineUserCountAsync();
        var connectionCount = await _connectionService.GetTotalConnectionCountAsync();

        return Ok(new
        {
            OnlineUsers = userCount,
            TotalConnections = connectionCount,
            Timestamp = DateTime.UtcNow,
            Status = "OK"
        });
    }

    /// <summary>
    /// 取得所有線上使用者詳細資訊
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetOnlineUsers()
    {
        var users = await _connectionService.GetAllOnlineUsersAsync();
        
        var result = users.Select(user => new
        {
            user.UserId,
            user.FirstConnectedAt,
            user.LastActivityAt,
            user.IsOnline,
            user.ConnectionCount,
            user.ActiveConnectionCount,
            Connections = user.Connections.Values.Select(conn => new
            {
                conn.ConnectionId,
                conn.ConnectedAt,
                conn.LastActivityAt,
                conn.IsConnected,
                conn.ClientIpAddress,
                conn.UserAgent
            })
        });

        return Ok(result);
    }

    /// <summary>
    /// 取得特定使用者狀態
    /// </summary>
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetUserStatus(string userId)
    {
        var userStatus = await _connectionService.GetUserStatusAsync(userId);
        
        if (userStatus == null)
        {
            return NotFound(new { Message = "使用者未找到或未連線" });
        }

        var result = new
        {
            userStatus.UserId,
            userStatus.FirstConnectedAt,
            userStatus.LastActivityAt,
            userStatus.IsOnline,
            userStatus.ConnectionCount,
            userStatus.ActiveConnectionCount,
            Connections = userStatus.Connections.Values.Select(conn => new
            {
                conn.ConnectionId,
                conn.ConnectedAt,
                conn.LastActivityAt,
                conn.IsConnected,
                conn.ClientIpAddress,
                conn.UserAgent
            })
        };

        return Ok(result);
    }

    /// <summary>
    /// 健康檢查端點
    /// </summary>
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "BasicApp.Chat.ConnectionService"
        });
    }
}
