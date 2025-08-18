using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.User.Commands;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    /// <summary>
    /// 註冊新使用者
    /// </summary>
    public async Task RegisterUser(string deviceFingerprint)
    {
        var httpContext = Context.GetHttpContext();
        var ip = httpContext?.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

        var userId = await _mediator.Send(
            new RegisterUserCommand(deviceFingerprint, Context.ConnectionId, ip, userAgent)
        );

        Context.Items["UserId"] = userId;

        // TODO:以下內容沒開規格，需要確認(暫時不要返回)
        // await Clients.Caller.SendAsync("UserRegistered", userId.ToString(), null, false);
        // await Clients.Caller.SendAsync("ConnectionEstablished", Context.ConnectionId, DateTime.UtcNow);
    }

    /// <summary>
    /// 更新使用者暱稱
    /// </summary>
    public async Task UpdateNickname(string newNickname)
    {
        var userId = Context.Items["UserId"] as Guid?;

        var result = await _mediator.Send(new UpdateNicknameCommand(userId ?? Guid.Empty, newNickname));

        if (result)
        {
            await Clients.Caller.SendAsync(
                "NicknameUpdated",
                newNickname,
                DateTime.UtcNow
            );
        }
        else
        {
            throw new HubException("Failed to update nickname");
        }
    }
}
