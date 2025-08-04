using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Monolithic.Shared.Logging;

/// <summary>
/// API 自動日誌記錄 Action Filter
/// </summary>
public class ApiLoggingActionFilter : ActionFilterAttribute
{
    private readonly IAppLogger<ApiLoggingActionFilter> _logger;
    private Stopwatch? _stopwatch;
    private string? _traceId;

    public ApiLoggingActionFilter(IAppLogger<ApiLoggingActionFilter> logger)
    {
        _logger = logger;
    }

    /// 用於記錄 Action 執行開始前的資訊
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        _traceId = context.HttpContext.TraceIdentifier;

        var controllerName = context.Controller.GetType().Name;
        var actionName = context.ActionDescriptor.RouteValues["action"] ?? "Unknown";
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        // 收集請求參數
        var requestData = new Dictionary<string, object?>();
        foreach (var param in context.ActionArguments)
        {
            requestData[param.Key] = param.Value;
        }
        if (context.HttpContext.Request.Query.Any())
        {
            requestData["QueryParams"] = context.HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        }

        _logger.LogInfo(
            $"[API請求] {controllerName} | {method} {path} | Action: {actionName}",
            new { Action = actionName, Data = requestData },
            _traceId
        );

        base.OnActionExecuting(context);
    }

    // 用於記錄 Action 執行結束後的資訊
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch?.Stop();
        var duration = _stopwatch?.Elapsed;

        var controllerName = context.Controller.GetType().Name;
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var statusCode = context.HttpContext.Response.StatusCode;

        if (context.Exception != null)
        {
            _logger.LogError(
                $"[API錯誤] {controllerName} | {method} {path} | Status: {statusCode} | Duration: {duration?.TotalMilliseconds}ms",
                context.Exception,
                null,
                _traceId
            );
        }
        else
        {
            _logger.LogInfo(
                $"[API回應] {controllerName} | {method} {path} | Status: {statusCode} | Duration: {duration?.TotalMilliseconds}ms",
                null,
                _traceId
            );
        }
        base.OnActionExecuted(context);
    }
}
