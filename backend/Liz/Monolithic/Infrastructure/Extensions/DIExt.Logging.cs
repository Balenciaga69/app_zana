using Monolithic.Shared.Logging;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
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
