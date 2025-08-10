using Microsoft.AspNetCore.SignalR;
using Monolithic.Features.Communication;
using Monolithic.Features.User.Repositories;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.User.Services;

public interface IUserCommunicationService
{
    Task UpdateNicknameAsync(string newNickname, string deviceFingerprint);
    Task RegisterUserAsync(string? existingUserId, string deviceFingerprint, string connectionId);
    Task HandleUserDisconnectedAsync(string connectionId);
}

public class UserCommunicationService : IUserCommunicationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<CommunicationHub> _hubContext;
    private readonly IAppLogger<UserCommunicationService> _logger;

    // 假設你有房間相關的 Repository（需要查詢用戶參與的房間）
    // private readonly IRoomParticipantRepository _roomParticipantRepository;

    public UserCommunicationService(
        IUserRepository userRepository,
        IHubContext<CommunicationHub> hubContext,
        IAppLogger<UserCommunicationService> logger
    )
    {
        _userRepository = userRepository;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// 更新暱稱並通知相關房間成員
    /// </summary>
    public async Task UpdateNicknameAsync(string newNickname, string deviceFingerprint)
    {
        if (string.IsNullOrEmpty(newNickname))
            throw new ArgumentException("新暱稱不能為空。");

        // 查詢 User
        var user = await _userRepository.GetByDeviceFingerprintAsync(deviceFingerprint);
        if (user == null)
            throw new InvalidOperationException("找不到對應的使用者，請確認裝置指紋是否正確。");

        var oldNickname = user.Nickname;
        user.Nickname = newNickname;
        await _userRepository.UpdateAsync(user);

        _logger.LogInfo(
            "[UserCommunication] 暱稱更新成功",
            new
            {
                UserId = user.Id,
                OldNickname = oldNickname,
                NewNickname = newNickname,
            },
            user.Id.ToString()
        );

        // 廣播給所有用戶（全域通知）
        await _hubContext.Clients.All.SendAsync("NicknameUpdated", user.Id, user.Nickname);

        // TODO: 如果只想通知同房間的人，需要：
        // 1. 查詢用戶參與的房間
        // 2. 對每個房間的 SignalR Group 發送通知
        // var userRooms = await _roomParticipantRepository.GetUserActiveRoomsAsync(user.Id);
        // foreach (var room in userRooms)
        // {
        //     await _hubContext.Clients.Group($"Room_{room.RoomId}").SendAsync("NicknameUpdated", user.Id, user.Nickname);
        // }
    }

    /// <summary>
    /// 註冊用戶（新用戶或重連）
    /// </summary>
    public async Task RegisterUserAsync(string? existingUserId, string deviceFingerprint, string connectionId)
    {
        _logger.LogInfo(
            "[UserCommunication] 用戶註冊請求",
            new
            {
                ExistingUserId = existingUserId,
                DeviceFingerprint = deviceFingerprint,
                ConnectionId = connectionId,
            },
            connectionId
        );

        // TODO: 實現用戶註冊/重連邏輯
        // 1. 檢查是否為重連（existingUserId + deviceFingerprint 驗證）
        // 2. 創建新用戶或恢復現有用戶
        // 3. 記錄 UserConnection
        // 4. 發送 UserRegistered 事件

        await _hubContext
            .Clients.Client(connectionId)
            .SendAsync("UserRegistered", "user123", "匿名用戶", true);
    }

    /// <summary>
    /// 處理用戶斷線
    /// </summary>
    public async Task HandleUserDisconnectedAsync(string connectionId)
    {
        _logger.LogInfo(
            "[UserCommunication] 處理用戶斷線",
            new { ConnectionId = connectionId },
            connectionId
        );

        // TODO: 實現斷線處理邏輯
        // 1. 查詢該 ConnectionId 對應的用戶
        // 2. 更新 UserConnection 的 DisconnectedAt
        // 3. 檢查用戶是否還有其他活躍連線
        // 4. 如果沒有，更新用戶狀態為離線
        // 5. 通知相關房間成員

        await Task.CompletedTask; // 暫時避免編譯錯誤
    }
}
