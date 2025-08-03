using Monolithic.Features.Identity.Services;

namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊 Identity 相關服務
    /// </summary>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        // 註冊 Identity 服務
        services.AddScoped<IIdentityService, IdentityService>();

        // TODO: 未來可能需要的服務
        // services.AddScoped<IDeviceFingerprintService, DeviceFingerprintService>();
        // services.AddScoped<IUserSessionManager, UserSessionManager>();

        return services;
    }
}
