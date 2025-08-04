using System.Diagnostics;

namespace Monolithic.Shared.Logging;

/// <summary>
/// 統一日誌服務實作
/// </summary>
/// <typeparam name="T">日誌來源類型</typeparam>
public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly string _serviceName;

    public AppLogger(ILogger<T> logger)
    {
        _logger = logger;
        _serviceName = typeof(T).Name;
    }

    public ILogger<T> Logger => _logger;

    public void LogInfo(string message, object? data = null, string? traceId = null)
    {
        _logger.LogInformation(
            "[Info] {ServiceName} | {Message} | Data: {@Data} | Trace: {TraceId}",
            _serviceName,
            message,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogWarn(string message, object? data = null, string? traceId = null)
    {
        _logger.LogWarning(
            "[Warn] {ServiceName} | {Message} | Data: {@Data} | Trace: {TraceId}",
            _serviceName,
            message,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogError(string message, Exception? exception = null, object? data = null, string? traceId = null)
    {
        _logger.LogError(
            exception,
            "[Error] {ServiceName} | {Message} | Data: {@Data} | Trace: {TraceId}",
            _serviceName,
            message,
            data,
            traceId ?? GetTraceId()
        );
    }

    private static string GetTraceId()
    {
        return Activity.Current?.Id ?? Guid.NewGuid().ToString("N")[..8];
    }
}
