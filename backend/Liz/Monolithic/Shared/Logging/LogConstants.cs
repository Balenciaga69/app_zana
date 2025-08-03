namespace Monolithic.Shared.Logging;

/// <summary>
/// 日誌常用操作常數
/// </summary>
public static class LogOperations
{
    // Identity Service 操作
    public const string UserCreated = "用戶建立";
    public const string UserRetrieved = "用戶查詢";
    public const string UserValidated = "用戶驗證";
    public const string UserActivityUpdated = "用戶活動更新";
    public const string UserStatusChanged = "用戶狀態變更";
    public const string FingerprintMatched = "指紋匹配";
    public const string FingerprintMismatch = "指紋不匹配";

    // Database 操作
    public const string DatabaseQuery = "資料庫查詢";
    public const string DatabaseInsert = "資料庫新增";
    public const string DatabaseUpdate = "資料庫更新";
    public const string DatabaseDelete = "資料庫刪除";

    // API 操作
    public const string ApiRequestReceived = "API請求接收";
    public const string ApiResponseSent = "API回應發送";
    public const string ValidationFailed = "參數驗證失敗";

    // Connection 操作
    public const string ConnectionEstablished = "連線建立";
    public const string ConnectionClosed = "連線關閉";
    public const string ReconnectionAttempt = "重新連線嘗試";

    // Room 操作 (未來使用)
    public const string RoomCreated = "房間建立";
    public const string RoomJoined = "加入房間";
    public const string RoomLeft = "離開房間";
    public const string RoomDestroyed = "房間銷毀";

    // Chat 操作 (未來使用)
    public const string MessageSent = "訊息發送";
    public const string MessageReceived = "訊息接收";
    public const string MessageStored = "訊息儲存";
}

/// <summary>
/// 安全事件類型常數
/// </summary>
public static class SecurityEvents
{
    public const string SuspiciousActivity = "可疑活動";
    public const string UnauthorizedAccess = "未授權存取";
    public const string FingerprintMismatch = "指紋不匹配";
    public const string MultipleFailedAttempts = "多次失敗嘗試";
    public const string IpAddressChanged = "IP位址變更";
}
