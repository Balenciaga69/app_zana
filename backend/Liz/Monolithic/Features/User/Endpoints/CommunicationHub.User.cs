using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Features.Communication;

public partial class CommunicationHub : Hub
{
    public async Task RegisterUser(string? existingUserId, string deviceFingerprint)
    {
        // TODO: 註冊或重新連線用戶
    }

    public async Task UpdateNickname(string newNickname)
    {
        // TODO: 即時更新暱稱
    }
}
