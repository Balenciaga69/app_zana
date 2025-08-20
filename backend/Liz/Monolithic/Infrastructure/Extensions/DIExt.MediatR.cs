using Infrastructure.Behaviors;
using MediatR;
using System.Reflection;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        // 註冊 MediatR，掃描當前組件中的所有 Handler
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // AddOpenBehavior 方法用於註冊開放式行為，這些行為會在處理請求時被調用
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }
}
