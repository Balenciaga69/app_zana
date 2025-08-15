using Microsoft.AspNetCore.SignalR;
using Monolithic.Shared.Common;
using Monolithic.Shared.Logging;

namespace Monolithic.Shared.SignalR;

/// <summary>
/// IHubFilter 實作：統一攔截 Hub 方法異常，紀錄日誌並回傳標準錯誤事件
/// TODO: 若需更複雜的行為（如 ForceDisconnect），再擴充
/// </summary>
public class HubExceptionFilter : IHubFilter
{
    private readonly IAppLogger<HubExceptionFilter> _logger;

    public HubExceptionFilter(IAppLogger<HubExceptionFilter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            // 結構化日誌
            var connectionId = invocationContext.Context.ConnectionId;
            var userId = invocationContext.Context.Items.TryGetValue("UserId", out var u) ? u : null;
            _logger.LogError(
                $"Hub method error: {invocationContext.HubMethodName}",
                ex,
                new { connectionId, userId }
            );

            // 決定錯誤代碼與友善訊息
            var (code, message) = MapExceptionToError(ex);

            // 嘗試向當前連線回報錯誤事件
            try
            {
                await invocationContext.Hub.Clients.Caller.SendAsync("Error", new { code, message });
            }
            catch
            {
                // 忽略向客戶端回報時的錯誤，已經在日誌中
            }

            // 若需要，可拋出自定義例外讓上層 middleware 處理
            return null;
        }
    }

    private static (string code, string message) MapExceptionToError(Exception ex)
    {
        return ex switch
        {
            UnauthorizedAccessException => (
                ErrorCode.AuthRequired.ToString(),
                ErrorMessages.GetMessage(ErrorCode.AuthRequired)
            ),
            ArgumentNullException => (
                ErrorCode.InvalidInput.ToString(),
                ErrorMessages.GetMessage(ErrorCode.InvalidInput)
            ),
            ArgumentException => (
                ErrorCode.InvalidInput.ToString(),
                ErrorMessages.GetMessage(ErrorCode.InvalidInput)
            ),
            KeyNotFoundException => (
                ErrorCode.ResourceNotFound.ToString(),
                ErrorMessages.GetMessage(ErrorCode.ResourceNotFound)
            ),
            _ => (
                ErrorCode.InternalServerError.ToString(),
                ErrorMessages.GetMessage(ErrorCode.InternalServerError)
            ),
        };
    }
}
