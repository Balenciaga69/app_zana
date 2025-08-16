using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.User.Commands;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    public async Task RegisterUser(string deviceFingerprint)
    {
        var userId = await _mediator.Send(new RegisterUserCommand(deviceFingerprint));

        await Clients.Caller.SendAsync("UserRegistered", userId.ToString(), null, false);
        await Clients.Caller.SendAsync("ConnectionEstablished", Context.ConnectionId, DateTime.UtcNow);
    }

    public Task UpdateNickname(string newNickname)
    {
        // TODO: 即時更新暱稱（實作於 Command/Handler 層）
        return Task.CompletedTask;
    }
}
