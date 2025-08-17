using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Repositories;

public interface IUserRepository
{
    Task<UserEntity?> GetByDeviceFingerprintAsync(string deviceFingerprint);
    Task<UserEntity?> GetByIdAsync(Guid userId);
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
}
