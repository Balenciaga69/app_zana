using BasicApp.Chat.Models.Connection;

namespace BasicApp.Chat.Services;

/// <summary>
/// 連線管理服務介面
/// </summary>
public interface IConnectionService
{
    /// <summary>
    /// 新增連線
    /// </summary>
    /// <param name="connectionId">SignalR 連線 ID</param>
    /// <param name="userId">使用者 ID，若為空則自動產生</param>
    /// <param name="clientIpAddress">客戶端 IP</param>
    /// <param name="userAgent">使用者代理</param>
    /// <returns>實際使用的 UserId</returns>
    Task<string> AddConnectionAsync(string connectionId, string? userId = null, string? clientIpAddress = null, string? userAgent = null);
    
    /// <summary>
    /// 移除連線
    /// </summary>
    /// <param name="connectionId">連線 ID</param>
    Task RemoveConnectionAsync(string connectionId);
    
    /// <summary>
    /// 透過連線 ID 取得使用者 ID
    /// </summary>
    /// <param name="connectionId">連線 ID</param>
    /// <returns>使用者 ID，若未找到則返回 null</returns>
    Task<string?> GetUserIdByConnectionAsync(string connectionId);
    
    /// <summary>
    /// 取得使用者的所有連線 ID
    /// </summary>
    /// <param name="userId">使用者 ID</param>
    /// <returns>連線 ID 列表</returns>
    Task<IEnumerable<string>> GetUserConnectionsAsync(string userId);
    
    /// <summary>
    /// 取得所有在線使用者數量
    /// </summary>
    Task<int> GetOnlineUserCountAsync();
    
    /// <summary>
    /// 取得總連線數
    /// </summary>
    Task<int> GetTotalConnectionCountAsync();
    
    /// <summary>
    /// 更新連線活動時間
    /// </summary>
    /// <param name="connectionId">連線 ID</param>
    Task UpdateLastActivityAsync(string connectionId);
    
    /// <summary>
    /// 取得使用者連線狀態
    /// </summary>
    /// <param name="userId">使用者 ID</param>
    Task<UserConnectionStatus?> GetUserStatusAsync(string userId);
    
    /// <summary>
    /// 取得所有在線使用者狀態
    /// </summary>
    Task<IEnumerable<UserConnectionStatus>> GetAllOnlineUsersAsync();
}
