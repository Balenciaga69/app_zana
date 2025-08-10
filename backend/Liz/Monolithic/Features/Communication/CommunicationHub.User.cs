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
            // 基本驗證
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "設備指紋不能為空");
                return;
            }

            // 委派給 Service 處理業務邏輯
            await _userCommunicationService.RegisterUserAsync(
                existingUserId,
                deviceFingerprint,
                connectionId
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
            // 基本驗證
            if (string.IsNullOrEmpty(newNickname))
            {
                await Clients.Caller.SendAsync("Error", "暱稱不能為空");
                return;
            }

            // 從 HttpContext 取得 deviceFingerprint（只信任伺服器端）
            var deviceFingerprint = Context.GetHttpContext()?.GetDeviceFingerprint();
            if (string.IsNullOrEmpty(deviceFingerprint))
            {
                await Clients.Caller.SendAsync("Error", "無法驗證裝置指紋");
                return;
            }

            // 委派給 Service 處理業務邏輯
            await _userCommunicationService.UpdateNicknameAsync(newNickname, deviceFingerprint);
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
            // 委派給 Service 處理用戶斷線邏輯
            await _userCommunicationService.HandleUserDisconnectedAsync(connectionId);
        }
        catch (Exception)
        {
            // ...existing code...
        }

        await base.OnDisconnectedAsync(exception);
    }
}
