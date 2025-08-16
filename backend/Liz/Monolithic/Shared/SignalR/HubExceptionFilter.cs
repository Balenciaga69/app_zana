using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Shared.SignalR;

public class HubExceptionFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext context,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(context);
        }
        catch (ValidationException vex)
        {
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

    public async ValueTask OnConnectedAsync(
        HubLifetimeContext context,
        Func<HubLifetimeContext, ValueTask> next)
    {
        await next(context);
    }

    public async ValueTask OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception? exception,
        Func<HubLifetimeContext, Exception?, ValueTask> next)
    {
        await next(context, exception);
    }

    private static bool ShouldAbortOnError(HubInvocationContext context)
    {
        return string.Equals(context.HubMethodName, "RegisterUser", StringComparison.OrdinalIgnoreCase);
    }

    private static Task SendErrorToCallerAsync(HubInvocationContext context, string message)
    {
        return context.Hub.Clients.Caller.SendAsync("Error", message);
    }
}
