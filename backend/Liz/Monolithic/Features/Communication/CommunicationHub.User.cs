using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    public async Task RegisterUser(string? existingUserId, string deviceFingerprint)
    {
        var connectionId = Context.ConnectionId;
        var http = Context.GetHttpContext();
        var ip = http?.Connection.RemoteIpAddress?.ToString();
        var userAgent = http?.Request.Headers["User-Agent"].ToString();

        var isNewUser = string.IsNullOrEmpty(existingUserId);
        var userId = isNewUser ? Guid.NewGuid().ToString() : existingUserId!;
        var nickname = $"匿名用戶{new Random().Next(1000, 9999)}";

        // 暫存到 Connection Items，供 UpdateNickname 使用
        Context.Items["UserId"] = userId;
        Context.Items["Nickname"] = nickname;
        Context.Items["DeviceFingerprint"] = deviceFingerprint;
        Context.Items["ConnectionId"] = connectionId;
        Context.Items["IpAddress"] = ip ?? string.Empty;
        Context.Items["UserAgent"] = userAgent ?? string.Empty;

        // TODO: 實作：儲存或更新 User 與 UserConnection 到 DB / Redis
        // TODO: 驗證 existingUserId 與 deviceFingerprint 綁定關係

        await Clients.Caller.SendAsync("UserRegistered", userId, nickname, isNewUser);
        await Clients.Caller.SendAsync("ConnectionEstablished", connectionId, DateTime.UtcNow);

        // 通知全域在線狀態改變（MVP: 最簡化通知）
        await Clients.Others.SendAsync("UserStatusChanged", userId, true, DateTime.UtcNow);
    }

    public async Task UpdateNickname(string newNickname)
    {
        if (string.IsNullOrWhiteSpace(newNickname))
        {
            await Clients.Caller.SendAsync("Error", "INVALID_INPUT: nickname empty");
            return;
        }

        if (!Context.Items.TryGetValue("UserId", out var uidObj) || uidObj is not string userId)
        {
            await Clients.Caller.SendAsync("Error", "AUTH_REQUIRED");
            return;
        }

        // TODO: 加入頻率限制檢查（暫未實作）
        // TODO: 儲存 nickname 到 DB

        Context.Items["Nickname"] = newNickname;

        await Clients.Caller.SendAsync("NicknameUpdated", userId, newNickname);
        // 廣播讓其他人知道此用戶暱稱更新（可依需求改為只通知房間）
        await Clients.Others.SendAsync("NicknameUpdated", userId, newNickname);
    }

    // NOTE: Heartbeat 已在 CommunicationHub.cs 實作，請勿在此 partial class 重複實作。
    // 若需特定的 Heartbeat 行為請修改主檔或以不同方法名補充。
}
