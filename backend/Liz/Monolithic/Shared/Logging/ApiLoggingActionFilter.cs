using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Monolithic.Shared.Logging;

/// <summary>
/// API 自動日誌記錄 Action Filter
/// </summary>
public class ApiLoggingActionFilter : ActionFilterAttribute
{
    private readonly ILogger<ApiLoggingActionFilter> _logger;
    private Stopwatch? _stopwatch;
    private string? _traceId;

    public ApiLoggingActionFilter(ILogger<ApiLoggingActionFilter> logger)
    {
        _logger = logger;
    }

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

        // Route parameters
        foreach (var param in context.ActionArguments)
        {
            requestData[param.Key] = param.Value;
        }

        // Query parameters
        if (context.HttpContext.Request.Query.Any())
        {
            requestData["QueryParams"] = context.HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        }

        _logger.LogInformation(
            "[API請求] {ControllerName} | {Method} {Path} | Action: {ActionName} | Data: {@RequestData} | Trace: {TraceId}",
            controllerName,
            method,
            path,
            actionName,
            requestData,
            _traceId
        );

        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch?.Stop();
        var duration = _stopwatch?.Elapsed;

        var controllerName = context.Controller.GetType().Name;
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var statusCode = context.HttpContext.Response.StatusCode;

        var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

        // 如果有例外，記錄錯誤日誌
        if (context.Exception != null)
        {
            _logger.LogError(
                context.Exception,
                "[API錯誤] {ControllerName} | {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | Trace: {TraceId}",
                controllerName,
                method,
                path,
                statusCode,
                duration?.TotalMilliseconds,
                _traceId
            );
        }
        else
        {
            _logger.Log(
                logLevel,
                "[API回應] {ControllerName} | {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | Trace: {TraceId}",
                controllerName,
                method,
                path,
                statusCode,
                duration?.TotalMilliseconds,
                _traceId
            );
        }

        base.OnActionExecuted(context);
    }
}
