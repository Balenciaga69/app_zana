using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Shared.SignalR;

public class HubExceptionFilter : IHubFilter
{
    /// <summary>
    /// 處理 SignalR 的方法調用
    /// </summary>
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext context,
        Func<HubInvocationContext, ValueTask<object?>> next
    )
    {
        try
        {
            return await next(context);
        }
        catch (ValidationException vex)
        {
            // 選取驗證錯誤訊息並組合成一個錯誤訊息
            var message = string.Join("; ", vex.Errors.Select(e => e.ErrorMessage));
            await SendErrorToCallerAsync(context, message);
            if (ShouldAbortOnError(context))
            {
                context.Context.Abort();
            }
            throw new HubException("Invocation failed");
        }
        catch (Exception)
        {
            await SendErrorToCallerAsync(context, "Internal server error");
            if (ShouldAbortOnError(context))
            {
                context.Context.Abort();
            }
            throw new HubException("Invocation failed");
        }
    }

    /// <summary>
    /// 處理 SignalR 的連接事件
    /// 值任務(ValueTask) 用於非同步操作，避免不必要的資源開銷
    /// </summary>
    public async ValueTask OnConnectedAsync(
        HubLifetimeContext context,
        Func<HubLifetimeContext, ValueTask> next
    )
    {
        await next(context);
    }

    /// <summary>
    /// 處理 SignalR 的斷開連接事件
    /// </summary>
    public async ValueTask OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception? exception,
        Func<HubLifetimeContext, Exception?, ValueTask> next
    )
    {
        await next(context, exception);
    }

    /// <summary>
    /// 檢查是否需要在錯誤時中止連接
    /// </summary>
    private static bool ShouldAbortOnError(HubInvocationContext context)
    {
        return string.Equals(context.HubMethodName, "RegisterUser", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 發送錯誤訊息給調用者
    /// </summary>
    private static Task SendErrorToCallerAsync(HubInvocationContext context, string message)
    {
        return context.Hub.Clients.Caller.SendAsync("Error", message);
    }
}
