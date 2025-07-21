namespace BasicApp.Chat.Models.Connection;

/// <summary>
/// 使用者連線狀態，追蹤單一使用者的所有連線
/// </summary>
public class UserConnectionStatus
{
    /// <summary>
    /// 使用者 ID
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 該使用者的所有連線（一個使用者可能同時有多個 tab 連線）
    /// </summary>
    public Dictionary<string, ConnectionInfo> Connections { get; set; } = new();

    /// <summary>
    /// 使用者首次連線時間
    /// </summary>
    public DateTime FirstConnectedAt { get; set; }

    /// <summary>
    /// 最後活動時間
    /// </summary>
    public DateTime LastActivityAt { get; set; }

    /// <summary>
    /// 是否有任何連線在線（至少一個連線）
    /// </summary>
    public bool IsOnline => Connections.Values.Any(c => c.IsConnected);

    /// <summary>
    /// 總連線數
    /// </summary>
    public int ConnectionCount => Connections.Count;

    /// <summary>
    /// 活躍連線數
    /// </summary>
    public int ActiveConnectionCount => Connections.Values.Count(c => c.IsConnected);
}
