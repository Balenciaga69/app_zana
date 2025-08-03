using Monolithic.Shared.Logging;

namespace Liz.Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊統一 Logger 服務
    /// </summary>
    public static IServiceCollection AddAppLogging(this IServiceCollection services)
    {
        // 註冊統一 Logger 作為 Singleton
        services.AddTransient(typeof(IAppLogger<>), typeof(AppLogger<>));

        return services;
    }
}
