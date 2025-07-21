namespace BasicApp.Chat.Models.Connection;

/// <summary>
/// 連線資訊模型，記錄每個 WebSocket 連線的詳細資訊
/// </summary>
public class ConnectionInfo
{
    /// <summary>
    /// SignalR 連線 ID（每次連線唯一）
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// 使用者 ID（瀏覽器綁定，存在 localStorage）
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// 連線建立時間
    /// </summary>
    public DateTime ConnectedAt { get; set; }

    /// <summary>
    /// 最後活動時間
    /// </summary>
    public DateTime LastActivityAt { get; set; }

    /// <summary>
    /// 是否已連線
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// 客戶端 IP 位址
    /// </summary>
    public string? ClientIpAddress { get; set; }

    /// <summary>
    /// 使用者代理字串
    /// </summary>
    public string? UserAgent { get; set; }
}
