using System.Reflection;
using MediatR;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
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
