using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Monolithic.Shared.Logging;

/// <summary>
/// 改善後的 API 自動日誌記錄 Action Filter - 遵循 Clean Code 原則
/// </summary>
public class ImprovedApiLoggingActionFilter : ActionFilterAttribute
{
    private readonly IAppLogger<ImprovedApiLoggingActionFilter> _logger;
    private Stopwatch? _stopwatch;
    private string? _traceId;
    private RequestContext? _requestContext;

    public ImprovedApiLoggingActionFilter(IAppLogger<ImprovedApiLoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
        _traceId = context.HttpContext.TraceIdentifier;
        _requestContext = ExtractRequestContext(context);

        LogRequest(_requestContext, context.ActionArguments, context.HttpContext.Request.Query);
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch?.Stop();
        var duration = _stopwatch?.Elapsed;
        var statusCode = context.HttpContext.Response.StatusCode;

        if (context.Exception != null)
        {
            LogError(_requestContext!, statusCode, duration, context.Exception);
        }
        else
        {
            LogResponse(_requestContext!, statusCode, duration);
        }

        base.OnActionExecuted(context);
    }

    // 提取公共的請求上下文資訊
    private RequestContext ExtractRequestContext(ActionExecutingContext context)
    {
        return new RequestContext(
            ControllerName: context.Controller.GetType().Name,
            ActionName: context.ActionDescriptor.RouteValues["action"] ?? "Unknown",
            Method: context.HttpContext.Request.Method,
            Path: context.HttpContext.Request.Path
        );
    }

    // 記錄請求日誌
    private void LogRequest(RequestContext requestContext, IDictionary<string, object?> actionArguments, IQueryCollection queryParams)
    {
        var requestData = BuildRequestData(actionArguments, queryParams);

        _logger.LogInfo(
            $"[API請求] {requestContext.ControllerName} | {requestContext.Method} {requestContext.Path} | Action: {requestContext.ActionName}",
            new { Action = requestContext.ActionName, Data = requestData },
            _traceId
        );
    }

    // 記錄成功回應日誌
    private void LogResponse(RequestContext requestContext, int statusCode, TimeSpan? duration)
    {
        _logger.LogInfo(
            $"[API回應] {requestContext.ControllerName} | {requestContext.Method} {requestContext.Path} | Status: {statusCode} | Duration: {duration?.TotalMilliseconds}ms",
            null,
            _traceId
        );
    }

    // 記錄錯誤日誌
    private void LogError(RequestContext requestContext, int statusCode, TimeSpan? duration, Exception exception)
    {
        _logger.LogError(
            $"[API錯誤] {requestContext.ControllerName} | {requestContext.Method} {requestContext.Path} | Status: {statusCode} | Duration: {duration?.TotalMilliseconds}ms",
            exception,
            null,
            _traceId
        );
    }

    // 建構請求資料
    private static Dictionary<string, object?> BuildRequestData(IDictionary<string, object?> actionArguments, IQueryCollection queryParams)
    {
        var requestData = new Dictionary<string, object?>();

        foreach (var param in actionArguments)
        {
            requestData[param.Key] = param.Value;
        }

        if (queryParams.Any())
        {
            requestData["QueryParams"] = queryParams.ToDictionary(q => q.Key, q => q.Value.ToString());
        }

        return requestData;
    }

    // 值物件來封裝請求上下文 - 遵循不可變性原則
    private record RequestContext(string ControllerName, string ActionName, string Method, string Path);
}
