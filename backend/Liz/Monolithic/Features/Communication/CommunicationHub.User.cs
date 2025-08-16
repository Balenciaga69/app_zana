using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.User.Commands;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    public async Task RegisterUser(string deviceFingerprint)
    {
        var httpContext = Context.GetHttpContext();
        var ip = httpContext?.Connection.RemoteIpAddress?.ToString();
        var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();

        var userId = await _mediator.Send(
            new RegisterUserCommand(deviceFingerprint, Context.ConnectionId, ip, userAgent)
        );
        await Clients.Caller.SendAsync("UserRegistered", userId.ToString(), null, false);
        await Clients.Caller.SendAsync("ConnectionEstablished", Context.ConnectionId, DateTime.UtcNow);
    }

    public Task UpdateNickname(string newNickname)
    {
        // TODO: 即時更新暱稱（實作於 Command/Handler 層）
        return Task.CompletedTask;
    }
}
