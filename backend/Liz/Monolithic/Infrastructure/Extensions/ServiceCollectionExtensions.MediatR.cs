// MediatR 相關 ServiceCollection 擴充
using System.Reflection;
using MediatR;

namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // 註冊 MediatR，掃描當前組件中的所有 Handler
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}
