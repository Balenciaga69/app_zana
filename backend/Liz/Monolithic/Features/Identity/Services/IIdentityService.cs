using Monolithic.Features.Identity.Models;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.Identity.Services;

public interface IIdentityService
{
    /// <summary>
    /// 建立新用戶或根據指紋找回現有用戶
    /// </summary>
    Task<UserSession> CreateOrRetrieveUserAsync(CreateUserRequest request);

    /// <summary>
    /// 根據 UserId 取得用戶資訊
    /// </summary>
    Task<UserSession?> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// 根據瀏覽器指紋查找用戶
    /// </summary>
    Task<UserSession?> FindUserByFingerprintAsync(FindUserByFingerprintRequest request);

    /// <summary>
    /// 驗證用戶身份（檢查 UserId 與指紋是否匹配）
    /// </summary>
    Task<bool> ValidateUserAsync(ValidateUserRequest request);

    /// <summary>
    /// 更新用戶活動時間與設備資訊
    /// </summary>
    Task UpdateUserActivityAsync(Guid userId, CreateUserRequest? deviceInfo = null);

    /// <summary>
    /// 設定用戶上線狀態
    /// </summary>
    Task SetUserOnlineStatusAsync(Guid userId, bool isOnline);
}
