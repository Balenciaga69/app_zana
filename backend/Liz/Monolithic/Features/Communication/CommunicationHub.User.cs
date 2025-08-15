using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    public async Task RegisterUser(string? existingUserId, string deviceFingerprint)
    {
        // TODO: 註冊或重新連線用戶
        // 1. 檢查 deviceFingerprint 是否有效
        // 2. 若 existingUserId 有值則驗證與 deviceFingerprint 綁定關係
        // 3. 若無 existingUserId 則建立新用戶
        // 4. 記錄連線資訊（IP、UserAgent、ConnectionId）
        // 5. 回傳 UserRegistered/ConnectionEstablished 事件
    }

    public async Task UpdateNickname(string newNickname)
    {
        // TODO: 即時更新暱稱
        // 1. 驗證 newNickname 格式與長度
        // 2. 取得當前 userId
        // 3. 更新資料庫/快取中的暱稱
        // 4. 廣播 NicknameUpdated 事件給相關用戶
    }
}
