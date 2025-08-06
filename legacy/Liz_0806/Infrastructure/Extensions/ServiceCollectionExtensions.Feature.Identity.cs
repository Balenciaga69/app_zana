namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊 Identity 相關服務
    /// </summary>
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        // 移除 IdentityService，業務邏輯改由 MediatR Handler 處理

        // TODO: 未來可能需要的服務
        // services.AddScoped<IDeviceFingerprintService, DeviceFingerprintService>();
        // services.AddScoped<IUserSessionManager, UserSessionManager>();

        return services;
    }
}
