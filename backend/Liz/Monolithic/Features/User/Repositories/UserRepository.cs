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
        // TODO: 未來加入 Redis 快取後，考慮先檢查快取中是否有資料，若無再查詢資料庫
        return await _context
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.DeviceFingerprint == deviceFingerprint && u.IsActive);
    }

    public async Task<User> CreateAsync(User user)
    {
        // TODO: 未來加入 Redis 快取後，新增用戶時需同步更新快取資料
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
        // TODO: 未來加入 Redis 快取後，更新用戶時需同步更新快取資料
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserOnlineAsync(Guid userId)
    {
        // TODO: 未來加入 Redis 快取後，考慮將在線狀態存入快取以提升查詢效率
        return await _context
            .UserConnections.AsNoTracking()
            .AnyAsync(uc => uc.UserId == userId.ToString() && uc.DisconnectedAt == null);
    }

    public async Task<IEnumerable<User>> GetOnlineUsersAsync()
    {
        // TODO: 未來加入 Redis 快取後，考慮將在線用戶列表存入快取以提升查詢效率
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
        // TODO: 未來加入 Redis 快取後，考慮將非活躍用戶的查詢結果存入快取以提升查詢效率
        return await _context
            .Users.AsNoTracking()
            .Where(u => u.LastActiveAt < inactiveSince && u.IsActive)
            .ToListAsync();
    }
}
