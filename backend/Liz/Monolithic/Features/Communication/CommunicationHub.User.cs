using MediatR;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Shared.Extensions;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    /// <summary>
    /// 註冊/重新連線用戶
    /// Hub 只負責驗證與轉發，業務邏輯委派給 Service
    /// </summary>
    public async Task RegisterUser(string? existingUserId, string deviceFingerprint)
    {
        var connectionId = Context.ConnectionId;
        try
        {
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "設備指紋不能為空");
                return;
            }

            // 直接透過 MediatR 呼叫 Command
            var mediator =
                Context.GetHttpContext()?.RequestServices.GetService(typeof(IMediator)) as IMediator;
            if (mediator == null)
            {
                await Clients.Caller.SendAsync("Error", "伺服器錯誤");
                return;
            }
            await mediator.Send(
                new Monolithic.Features.User.Commands.RegisterUserCommand(
                    existingUserId,
                    deviceFingerprint,
                    connectionId
                )
            );
        }
        catch (Exception)
        {
            await Clients.Caller.SendAsync("Error", "註冊失敗");
        }
    }

    /// <summary>
    /// 即時更新暱稱
    /// Hub 只負責驗證與轉發，業務邏輯委派給 Service
    /// </summary>
    public async Task UpdateNickname(string newNickname)
    {
        var connectionId = Context.ConnectionId;
        try
        {
            if (string.IsNullOrEmpty(newNickname))
            {
                await Clients.Caller.SendAsync("Error", "暱稱不能為空");
                return;
            }
            var deviceFingerprint = Context.GetHttpContext()?.GetDeviceFingerprint();
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "無法驗證裝置指紋");
                return;
            }
            var mediator =
                Context.GetHttpContext()?.RequestServices.GetService(typeof(IMediator)) as IMediator;
            if (mediator == null)
            {
                await Clients.Caller.SendAsync("Error", "伺服器錯誤");
                return;
            }
            await mediator.Send(new Monolithic.Features.User.Commands.UpdateNicknameCommand(newNickname));
        }
        catch (Exception)
        {
            await Clients.Caller.SendAsync("Error", "暱稱更新失敗");
        }
    }

    /// <summary>
    /// 連線中斷事件
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        try
        {
            var mediator =
                Context.GetHttpContext()?.RequestServices.GetService(typeof(IMediator)) as IMediator;
            if (mediator != null)
            {
                await mediator.Send(
                    new Monolithic.Features.User.Commands.HandleUserDisconnectedCommand(
                        connectionId,
                        exception?.Message
                    )
                );
            }
        }
        catch (Exception)
        {
            // ...existing code...
        }
        await base.OnDisconnectedAsync(exception);
    }
}
