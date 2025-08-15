namespace Monolithic.Features.User.Repositories;

using UserConnection = Infrastructure.Data.Entities.UserConnection;

public interface IUserConnectionRepository
{
    Task<UserConnection?> GetByConnectionIdAsync(string connectionId);
    Task<IEnumerable<UserConnection>> GetConnectionsByUserIdAsync(Guid userId);
    Task AddConnectionAsync(UserConnection connection);
    Task UpdateConnectionAsync(UserConnection connection);
    Task<IEnumerable<UserConnection>> GetActiveConnectionsAsync();
    Task<IEnumerable<UserConnection>> GetDisconnectedConnectionsAsync(DateTime disconnectedBefore);
    Task RemoveConnectionAsync(string connectionId);
}
