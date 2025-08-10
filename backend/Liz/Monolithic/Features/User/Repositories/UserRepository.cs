using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data;

namespace Monolithic.Features.User.Repositories;

using User = Infrastructure.Data.Entities.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        // 查詢資料庫，根據 userId 取得 User 實體，找不到則回傳 null
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
    }

    public async Task<User?> GetByDeviceFingerprintAsync(string deviceFingerprint)
    {
        // 查詢資料庫，根據 deviceFingerprint 取得 User 實體，找不到則回傳 null
        return await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.DeviceFingerprint == deviceFingerprint && u.IsActive);
    }

    public async Task<User> CreateAsync(User user)
    {
        // 將新的 User 實體新增到資料庫，儲存後回傳該 User
        // 檢查 deviceFingerprint 是否已存在
        var existingUser = await GetByDeviceFingerprintAsync(user.DeviceFingerprint);
        if (existingUser != null)
        {
            throw new InvalidOperationException(
                $"User with device fingerprint '{user.DeviceFingerprint}' already exists"
            );
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        // 更新資料庫中的 User 實體
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserOnlineAsync(Guid userId)
    {
        // 查詢該用戶是否有活躍的連線
        return await _context
            .UserConnections.AsNoTracking()
            .AnyAsync(uc => uc.UserId == userId.ToString() && uc.DisconnectedAt == null);
    }

    public async Task<IEnumerable<User>> GetOnlineUsersAsync()
    {
        // 查詢所有目前在線的 User（根據 UserConnection 判斷）
        var onlineUserIds = await _context
            .UserConnections.AsNoTracking()
            .Where(uc => uc.DisconnectedAt == null)
            .Select(uc => uc.UserId)
            .Distinct()
            .ToListAsync();

        return await _context
            .Users.AsNoTracking()
            .Where(u => onlineUserIds.Contains(u.Id.ToString()) && u.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetInactiveUsersAsync(DateTime inactiveSince)
    {
        // 查詢自指定時間以來未活動的用戶
        return await _context
            .Users.AsNoTracking()
            .Where(u => u.LastActiveAt < inactiveSince && u.IsActive)
            .ToListAsync();
    }
}
