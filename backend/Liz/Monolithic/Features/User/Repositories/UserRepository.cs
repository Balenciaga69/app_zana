using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Repositories;

/// <summary>
/// 用戶資料存取介面
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 根據 ID 取得用戶
    /// </summary>
    Task<Infrastructure.Data.Entities.User?> GetByIdAsync(Guid id);

    /// <summary>
    /// 根據設備指紋取得用戶
    /// </summary>
    Task<Infrastructure.Data.Entities.User?> GetByDeviceFingerprintAsync(string deviceFingerprint);

    /// <summary>
    /// 創建新用戶
    /// </summary>
    Task<Infrastructure.Data.Entities.User> CreateAsync(Infrastructure.Data.Entities.User user);

    /// <summary>
    /// 更新用戶
    /// </summary>
    Task<Infrastructure.Data.Entities.User> UpdateAsync(Infrastructure.Data.Entities.User user);

    /// <summary>
    /// 取得用戶連線歷史
    /// </summary>
    Task<IEnumerable<UserConnection>> GetUserConnectionsAsync(Guid userId, int skip, int take);

    /// <summary>
    /// 取得用戶連線總數
    /// </summary>
    Task<int> GetUserConnectionsCountAsync(Guid userId);

    /// <summary>
    /// 檢查用戶是否在線
    /// </summary>
    Task<bool> IsUserOnlineAsync(Guid userId);

    /// <summary>
    /// 創建用戶連線記錄
    /// </summary>
    Task<UserConnection> CreateConnectionAsync(UserConnection connection);

    /// <summary>
    /// 更新用戶連線記錄
    /// </summary>
    Task<UserConnection> UpdateConnectionAsync(UserConnection connection);

    /// <summary>
    /// 取得用戶的活躍連線
    /// </summary>
    Task<IEnumerable<UserConnection>> GetActiveConnectionsAsync(Guid userId);
}

/// <summary>
/// 用戶資料存取實作
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Infrastructure.Data.Entities.User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Infrastructure.Data.Entities.User?> GetByDeviceFingerprintAsync(string deviceFingerprint)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.DeviceFingerprint == deviceFingerprint);
    }

    public async Task<Infrastructure.Data.Entities.User> CreateAsync(Infrastructure.Data.Entities.User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Infrastructure.Data.Entities.User> UpdateAsync(Infrastructure.Data.Entities.User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<IEnumerable<UserConnection>> GetUserConnectionsAsync(Guid userId, int skip, int take)
    {
        return await _context
            .UserConnections.Where(c => c.UserId == userId.ToString())
            .OrderByDescending(c => c.ConnectedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetUserConnectionsCountAsync(Guid userId)
    {
        return await _context.UserConnections.CountAsync(c => c.UserId == userId.ToString());
    }

    public async Task<bool> IsUserOnlineAsync(Guid userId)
    {
        return await _context.UserConnections.AnyAsync(c => c.UserId == userId.ToString() && c.DisconnectedAt == null);
    }

    public async Task<UserConnection> CreateConnectionAsync(UserConnection connection)
    {
        _context.UserConnections.Add(connection);
        await _context.SaveChangesAsync();
        return connection;
    }

    public async Task<UserConnection> UpdateConnectionAsync(UserConnection connection)
    {
        _context.UserConnections.Update(connection);
        await _context.SaveChangesAsync();
        return connection;
    }

    public async Task<IEnumerable<UserConnection>> GetActiveConnectionsAsync(Guid userId)
    {
        return await _context.UserConnections.Where(c => c.UserId == userId.ToString() && c.DisconnectedAt == null).ToListAsync();
    }
}
