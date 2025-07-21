using BasicApp.Chat.Models.Connection;
using System.Collections.Concurrent;
using ConnectionInfo = BasicApp.Chat.Models.Connection.ConnectionInfo;

namespace BasicApp.Chat.Services;

/// <summary>
/// 連線管理服務實作（記憶體版本，適合 MVP 階段）
/// </summary>
public class ConnectionService : IConnectionService
{
    // 使用 ConcurrentDictionary 確保線程安全
    private readonly ConcurrentDictionary<string, UserConnectionStatus> _userConnections = new();
    private readonly ConcurrentDictionary<string, string> _connectionToUser = new();

    private readonly ILogger<ConnectionService> _logger;

    public ConnectionService(ILogger<ConnectionService> logger)
    {
        _logger = logger;
    }

    public async Task<string> AddConnectionAsync(string connectionId, string? userId = null, string? clientIpAddress = null, string? userAgent = null)
    {
        await Task.CompletedTask; // 暫時保持異步簽章，未來可能需要資料庫操作

        // 如果沒有提供 UserId，則產生新的
        if (string.IsNullOrWhiteSpace(userId))
        {
            userId = Guid.NewGuid().ToString();
            _logger.LogInformation("Generated new UserId: {UserId} for ConnectionId: {ConnectionId}", userId, connectionId);
        }

        var now = DateTime.UtcNow;
        var connectionInfo = new ConnectionInfo
        {
            ConnectionId = connectionId,
            UserId = userId,
            ConnectedAt = now,
            LastActivityAt = now,
            IsConnected = true,
            ClientIpAddress = clientIpAddress,
            UserAgent = userAgent
        };

        // 更新連線對應表
        _connectionToUser.TryAdd(connectionId, userId);

        // 更新使用者連線狀態
        _userConnections.AddOrUpdate(
            userId,
            // 如果是新使用者
            key => new UserConnectionStatus
            {
                UserId = userId,
                FirstConnectedAt = now,
                LastActivityAt = now,
                Connections = new Dictionary<string, ConnectionInfo> { { connectionId, connectionInfo } }
            },
            // 如果使用者已存在，新增連線
            (key, existingStatus) =>
            {
                existingStatus.Connections[connectionId] = connectionInfo;
                existingStatus.LastActivityAt = now;
                return existingStatus;
            });

        _logger.LogInformation("Added connection {ConnectionId} for user {UserId}. Total connections for user: {Count}",
            connectionId, userId, _userConnections[userId].ConnectionCount);

        return userId;
    }

    public async Task RemoveConnectionAsync(string connectionId)
    {
        await Task.CompletedTask; // 保持異步簽章

        if (!_connectionToUser.TryRemove(connectionId, out var userId))
        {
            _logger.LogWarning("Attempted to remove non-existent connection: {ConnectionId}", connectionId);
            return;
        }

        if (_userConnections.TryGetValue(userId, out var userStatus))
        {
            userStatus.Connections.Remove(connectionId);

            // 如果使用者沒有任何連線了，移除使用者狀態
            if (userStatus.ConnectionCount == 0)
            {
                _userConnections.TryRemove(userId, out _);
                _logger.LogInformation("Removed user {UserId} (no more connections)", userId);
            }
            else
            {
                _logger.LogInformation("Removed connection {ConnectionId} for user {UserId}. Remaining connections: {Count}",
                    connectionId, userId, userStatus.ConnectionCount);
            }
        }
    }

    public async Task<string?> GetUserIdByConnectionAsync(string connectionId)
    {
        await Task.CompletedTask; // 保持異步簽章
        _connectionToUser.TryGetValue(connectionId, out var userId);
        return userId;
    }

    public async Task<IEnumerable<string>> GetUserConnectionsAsync(string userId)
    {
        await Task.CompletedTask; // 保持異步簽章

        if (_userConnections.TryGetValue(userId, out var userStatus))
        {
            return userStatus.Connections.Keys.ToList();
        }

        return Enumerable.Empty<string>();
    }

    public async Task<int> GetOnlineUserCountAsync()
    {
        await Task.CompletedTask; // 保持異步簽章
        return _userConnections.Count;
    }

    public async Task<int> GetTotalConnectionCountAsync()
    {
        await Task.CompletedTask; // 保持異步簽章
        return _connectionToUser.Count;
    }

    public async Task UpdateLastActivityAsync(string connectionId)
    {
        await Task.CompletedTask; // 保持異步簽章

        if (_connectionToUser.TryGetValue(connectionId, out var userId) &&
            _userConnections.TryGetValue(userId, out var userStatus) &&
            userStatus.Connections.TryGetValue(connectionId, out var connectionInfo))
        {
            var now = DateTime.UtcNow;
            connectionInfo.LastActivityAt = now;
            userStatus.LastActivityAt = now;
        }
    }

    public async Task<UserConnectionStatus?> GetUserStatusAsync(string userId)
    {
        await Task.CompletedTask; // 保持異步簽章
        _userConnections.TryGetValue(userId, out var userStatus);
        return userStatus;
    }

    public async Task<IEnumerable<UserConnectionStatus>> GetAllOnlineUsersAsync()
    {
        await Task.CompletedTask; // 保持異步簽章
        return _userConnections.Values.ToList();
    }
}
