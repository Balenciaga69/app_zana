using System.Net;
using System.Text.Json;
using Monolithic.Shared.Common;

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

        // 如果異常訊息不為空 or 不等於預設訊息 ? 例外訊息 : 預設訊息
        var message =
            (!string.IsNullOrWhiteSpace(exception.Message) && exception.Message != defaultMessage)
                ? exception.Message
                : defaultMessage;

        object errors;
        // 如果是 FluentValidation 的驗證異常，則提取錯誤訊息
        if (exception is FluentValidation.ValidationException validationEx)
        {
            errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
        }
        // 否則返回異常訊息和堆疊追蹤
        else
        {
            errors = new { exception.Message, exception.StackTrace };
        }

        // 統一 API 響應格式
        var response = ApiResponse<object>.Fail(errorCode, message, errors: errors);
        response.TraceId = context.TraceIdentifier;

        // 將錯誤資訊序列化為 JSON 並寫入響應
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
        if (exception is FluentValidation.ValidationException)
        {
            // 400 BadRequest, InvalidInput, 錯誤訊息統一
            return (HttpStatusCode.BadRequest, ErrorCode.InvalidInput, "輸入資料驗證失敗");
        }

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
