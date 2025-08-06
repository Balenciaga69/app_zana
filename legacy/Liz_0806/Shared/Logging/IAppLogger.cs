namespace Monolithic.Shared.Logging;

/// <summary>
/// 應用程式統一日誌介面
/// </summary>
/// <typeparam name="T">日誌來源類型</typeparam>
public interface IAppLogger<T>
{
    /// <summary>
    /// 記錄一般資訊日誌
    /// </summary>
    void LogInfo(string message, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄警告日誌
    /// </summary>
    void LogWarn(string message, object? data = null, string? traceId = null);

    /// <summary>
    /// 記錄錯誤日誌
    /// </summary>
    void LogError(string message, Exception? exception = null, object? data = null, string? traceId = null);

    /// <summary>
    /// 原始 ILogger 介面（緊急時使用）
    /// </summary>
    ILogger<T> Logger { get; }
}
