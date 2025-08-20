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

        if (httpContext != null)
        {
            httpContext.Response.Cookies.Append(
                "UserId",
                userId.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true, // 若有 https
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                }
            );
        }

        // TODO:以下內容沒開規格，需要確認(暫時不要返回)
        // await Clients.Caller.SendAsync("UserRegistered", userId.ToString(), null, false);
        // await Clients.Caller.SendAsync("ConnectionEstablished", Context.ConnectionId, DateTime.UtcNow);
    }

    /// <summary>
    /// 更新使用者暱稱
    /// </summary>
    public async Task UpdateNickname(string newNickname)
    {
        // @Copilot FIXME: 這邊需要移除，換成Restful API 且業務邏輯也換了
        var userId = Context.Items["UserId"] as Guid?;

        var result = await _mediator.Send(new UpdateNicknameCommand(userId ?? Guid.Empty, newNickname));

        if (result)
        {
            // 將新的暱稱發送給呼叫者本身
            await Clients.Caller.SendAsync("NicknameUpdated", newNickname);
        }
        else
        {
            throw new HubException("Failed to update nickname");
        }
    }
}
