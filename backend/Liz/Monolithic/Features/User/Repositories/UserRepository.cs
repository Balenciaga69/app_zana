using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UserEntity?> GetByDeviceFingerprintAsync(string deviceFingerprint)
    {
        return await _db
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.DeviceFingerprint == deviceFingerprint);
    }

    public async Task<UserEntity?> GetByIdAsync(Guid userId)
    {
        return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task AddAsync(UserEntity user)
    {
        await _db.Users.AddAsync(user);
    }

    public Task UpdateAsync(UserEntity user)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }
}
