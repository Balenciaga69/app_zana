using Monolithic.Infrastructure.Data.Entities;

namespace Monolithic.Features.User.Repositories;

public interface IUserConnectionRepository
{
    Task AddAsync(UserConnectionEntity connection);
}
