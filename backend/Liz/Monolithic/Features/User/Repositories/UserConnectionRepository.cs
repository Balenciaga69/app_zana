using Monolithic.Infrastructure.Data;
using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Repositories;

public class UserConnectionRepository : IUserConnectionRepository
{
    private readonly AppDbContext _db;

    public UserConnectionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(UserConnectionEntity connection)
    {
        await _db.UserConnections.AddAsync(connection);
    }
}
