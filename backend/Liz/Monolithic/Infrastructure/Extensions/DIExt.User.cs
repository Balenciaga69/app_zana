using Monolithic.Features.User.Repositories;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
{
    /// <summary>
    /// 註冊 User Feature 相關服務
    /// </summary>
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        // 註冊 User Repository
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
