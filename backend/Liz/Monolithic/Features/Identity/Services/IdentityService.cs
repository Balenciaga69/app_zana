using Monolithic.Features.Identity.Models;
using Monolithic.Infrastructure.Data;
using Monolithic.Shared.Logging;

namespace Monolithic.Features.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly AppDbContext _context;
    private readonly IAppLogger<IdentityService> _appLogger;

    public IdentityService(AppDbContext context, IAppLogger<IdentityService> appLogger)
    {
        _context = context;
        _appLogger = appLogger;
    }

    public async Task<UserSession> CreateOrRetrieveUserAsync(CreateUserRequest request)
    {
        _appLogger.LogInfo($"嘗試建立或找回用戶", new { request.BrowserFingerprint, request.IpAddress });
        await Task.CompletedTask;
        throw new NotImplementedException("CreateOrRetrieveUserAsync 尚未實作");
    }

    public async Task<UserSession?> GetUserByIdAsync(Guid userId)
    {
        _appLogger.LogInfo($"查詢用戶資訊", new { UserId = userId });
        await Task.CompletedTask;
        throw new NotImplementedException("GetUserByIdAsync 尚未實作");
    }

    public async Task<UserSession?> FindUserByFingerprintAsync(FindUserByFingerprintRequest request)
    {
        _appLogger.LogInfo($"依指紋查找用戶", new { request.BrowserFingerprint, request.IpAddress });
        await Task.CompletedTask;
        throw new NotImplementedException("FindUserByFingerprintAsync 尚未實作");
    }

    public async Task<bool> ValidateUserAsync(ValidateUserRequest request)
    {
        _appLogger.LogInfo($"驗證用戶身份", new { request.UserId, request.BrowserFingerprint });
        await Task.CompletedTask;
        throw new NotImplementedException("ValidateUserAsync 尚未實作");
    }

    public async Task UpdateUserActivityAsync(Guid userId, CreateUserRequest? deviceInfo = null)
    {
        _appLogger.LogInfo($"更新用戶活動時間", new { UserId = userId, DeviceInfo = deviceInfo });
        await Task.CompletedTask;
        throw new NotImplementedException("UpdateUserActivityAsync 尚未實作");
    }

    public async Task SetUserOnlineStatusAsync(Guid userId, bool isOnline)
    {
        _appLogger.LogInfo($"設定用戶上線狀態", new { UserId = userId, IsOnline = isOnline });
        await Task.CompletedTask;
        throw new NotImplementedException("SetUserOnlineStatusAsync 尚未實作");
    }

    // TODO: 私有輔助方法
    // private UserSession MapToUserSession(User user) { ... }
    // private void UpdateUserDeviceInfo(User user, CreateUserRequest request) { ... }
}
