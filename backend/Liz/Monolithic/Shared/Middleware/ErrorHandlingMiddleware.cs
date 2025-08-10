using Monolithic.Shared.Common;
using System.Net;
using System.Text.Json;

namespace Monolithic.Shared.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 處理請求
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// 處理異常並返回統一的 API 響應格式
    /// </summary>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorCode, defaultMessage) = GetErrorDetails(exception);
        context.Response.StatusCode = (int)statusCode;

        var message =
            (!string.IsNullOrWhiteSpace(exception.Message) && exception.Message != defaultMessage)
                ? exception.Message
                : defaultMessage;

        var response = ApiResponse<object>.Fail(
            errorCode,
            message,
            errors: new { exception.Message, exception.StackTrace }
        );
        response.TraceId = context.TraceIdentifier;

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// 根據異常類型決定 HTTP 狀態碼和錯誤碼
    /// </summary>
    private static (HttpStatusCode statusCode, ErrorCode errorCode, string message) GetErrorDetails(
        Exception exception
    )
    {
        return exception switch
        {
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                ErrorCode.AuthRequired,
                ErrorMessages.GetMessage(ErrorCode.AuthRequired)
            ),
            InvalidOperationException => (
                HttpStatusCode.NotFound,
                ErrorCode.NotFound,
                ErrorMessages.GetMessage(ErrorCode.NotFound)
            ),
            ArgumentNullException => (
                HttpStatusCode.BadRequest,
                ErrorCode.InvalidInput,
                ErrorMessages.GetMessage(ErrorCode.InvalidInput)
            ),
            ArgumentException => (
                HttpStatusCode.BadRequest,
                ErrorCode.InvalidInput,
                ErrorMessages.GetMessage(ErrorCode.InvalidInput)
            ),
            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                ErrorCode.ResourceNotFound,
                ErrorMessages.GetMessage(ErrorCode.ResourceNotFound)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ErrorCode.InternalServerError,
                ErrorMessages.GetMessage(ErrorCode.InternalServerError)
            ),
        };
    }
}
