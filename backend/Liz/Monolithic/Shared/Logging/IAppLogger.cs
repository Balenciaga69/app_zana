namespace Monolithic.Shared.Logging;

/// <summary>
/// 應用程式統一日誌介面
/// </summary>
/// <typeparam name="T">日誌來源類型</typeparam>
public interface IAppLogger<T>
{
    /// <summary>
    /// 記錄用戶操作日誌
    /// </summary>
    void LogUserAction(Guid? userId, string action, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄業務邏輯日誌
    /// </summary>
    void LogBusinessInfo(string operation, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄業務警告
    /// </summary>
    void LogBusinessWarning(string operation, string reason, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄業務錯誤
    /// </summary>
    void LogBusinessError(string operation, Exception? exception = null, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄系統錯誤
    /// </summary>
    void LogSystemError(Exception exception, object? context = null, string? traceId = null);

    /// <summary>
    /// 記錄資料庫操作
    /// </summary>
    void LogDatabaseOperation(string operation, string entityType, object? entityId = null, TimeSpan? duration = null, string? traceId = null);

    /// <summary>
    /// 記錄 API 請求日誌
    /// </summary>
    void LogApiRequest(string method, string path, object? requestData = null, string? traceId = null);

    /// <summary>
    /// 記錄 API 回應日誌
    /// </summary>
    void LogApiResponse(string method, string path, int statusCode, TimeSpan? duration = null, string? traceId = null);

    /// <summary>
    /// 記錄效能指標
    /// </summary>
    void LogPerformance(string operation, TimeSpan duration, object? metadata = null, string? traceId = null);

    /// <summary>
    /// 記錄安全相關事件
    /// </summary>
    void LogSecurityEvent(string eventType, string details, Guid? userId = null, string? ipAddress = null, string? traceId = null);

    /// <summary>
    /// 原始 ILogger 介面（緊急時使用）
    /// </summary>
    ILogger<T> Logger { get; }
}
