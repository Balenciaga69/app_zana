namespace Monolithic.Shared.Logging;

/// <summary>
/// 應用程式統一日誌介面
/// </summary>
/// <typeparam name="T">日誌來源類型</typeparam>
public interface IAppLogger<T>
{
    void LogInfo(string message, object? data = null, string? traceId = null);

    void LogWarn(string message, object? data = null, string? traceId = null);

    void LogError(string message, Exception? exception = null, object? data = null, string? traceId = null);

    ILogger<T> Logger { get; }
}
