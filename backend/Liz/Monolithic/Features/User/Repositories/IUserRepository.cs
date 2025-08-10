namespace Monolithic.Features.User.Repositories;

using User = Infrastructure.Data.Entities.User;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByDeviceFingerprintAsync(string deviceFingerprint);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> IsUserOnlineAsync(Guid userId);
    Task<IEnumerable<User>> GetOnlineUsersAsync();
    Task<IEnumerable<User>> GetInactiveUsersAsync(DateTime inactiveSince);
}
