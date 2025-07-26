using BasicApp.Chat.Services;

namespace BasicApp.Chat.Extensions;

/// <summary>
/// 服務註冊擴充方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊連線管理相關服務
    /// </summary>
    public static IServiceCollection AddConnectionServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionService, ConnectionService>();
        return services;
    }
}
