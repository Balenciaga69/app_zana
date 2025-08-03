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

    public void LogUserAction(Guid? userId, string action, object? data = null, string? traceId = null)
    {
        _logger.LogInformation(
            "[用戶操作] {ServiceName} | 用戶: {UserId} | 操作: {Action} | 資料: {@Data} | 追蹤: {TraceId}",
            _serviceName,
            userId,
            action,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogBusinessInfo(string operation, object? data = null, string? traceId = null)
    {
        _logger.LogInformation(
            "[業務邏輯] {ServiceName} | 操作: {Operation} | 資料: {@Data} | 追蹤: {TraceId}",
            _serviceName,
            operation,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogBusinessWarning(string operation, string reason, object? data = null, string? traceId = null)
    {
        _logger.LogWarning(
            "[業務警告] {ServiceName} | 操作: {Operation} | 原因: {Reason} | 資料: {@Data} | 追蹤: {TraceId}",
            _serviceName,
            operation,
            reason,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogBusinessError(string operation, Exception? exception = null, object? data = null, string? traceId = null)
    {
        _logger.LogError(
            exception,
            "[業務錯誤] {ServiceName} | 操作: {Operation} | 資料: {@Data} | 追蹤: {TraceId}",
            _serviceName,
            operation,
            data,
            traceId ?? GetTraceId()
        );
    }

    public void LogSystemError(Exception exception, object? context = null, string? traceId = null)
    {
        _logger.LogError(
            exception,
            "[系統錯誤] {ServiceName} | 上下文: {@Context} | 追蹤: {TraceId}",
            _serviceName,
            context,
            traceId ?? GetTraceId()
        );
    }

    public void LogDatabaseOperation(string operation, string entityType, object? entityId = null, TimeSpan? duration = null, string? traceId = null)
    {
        _logger.LogInformation(
            "[資料庫] {ServiceName} | 操作: {Operation} | 實體: {EntityType} | ID: {EntityId} | 耗時: {Duration}ms | 追蹤: {TraceId}",
            _serviceName,
            operation,
            entityType,
            entityId,
            duration?.TotalMilliseconds,
            traceId ?? GetTraceId()
        );
    }

    public void LogApiRequest(string method, string path, object? requestData = null, string? traceId = null)
    {
        _logger.LogInformation(
            "[API請求] {ServiceName} | {Method} {Path} | 資料: {@RequestData} | 追蹤: {TraceId}",
            _serviceName,
            method,
            path,
            requestData,
            traceId ?? GetTraceId()
        );
    }

    public void LogApiResponse(string method, string path, int statusCode, TimeSpan? duration = null, string? traceId = null)
    {
        var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

        _logger.Log(
            logLevel,
            "[API回應] {ServiceName} | {Method} {Path} | 狀態: {StatusCode} | 耗時: {Duration}ms | 追蹤: {TraceId}",
            _serviceName,
            method,
            path,
            statusCode,
            duration?.TotalMilliseconds,
            traceId ?? GetTraceId()
        );
    }

    public void LogPerformance(string operation, TimeSpan duration, object? metadata = null, string? traceId = null)
    {
        var logLevel = duration.TotalMilliseconds > 1000 ? LogLevel.Warning : LogLevel.Information;

        _logger.Log(
            logLevel,
            "[效能監控] {ServiceName} | 操作: {Operation} | 耗時: {Duration}ms | 元資料: {@Metadata} | 追蹤: {TraceId}",
            _serviceName,
            operation,
            duration.TotalMilliseconds,
            metadata,
            traceId ?? GetTraceId()
        );
    }

    public void LogSecurityEvent(string eventType, string details, Guid? userId = null, string? ipAddress = null, string? traceId = null)
    {
        _logger.LogWarning(
            "[安全事件] {ServiceName} | 事件: {EventType} | 詳情: {Details} | 用戶: {UserId} | IP: {IpAddress} | 追蹤: {TraceId}",
            _serviceName,
            eventType,
            details,
            userId,
            ipAddress,
            traceId ?? GetTraceId()
        );
    }

    private static string GetTraceId()
    {
        return Activity.Current?.Id ?? Guid.NewGuid().ToString("N")[..8];
    }
}
