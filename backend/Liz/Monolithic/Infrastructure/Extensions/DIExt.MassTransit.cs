using MassTransit;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
{
    /// <summary>
    /// 註冊 MassTransit 服務
    /// </summary>
    public static IServiceCollection AddMassTransitServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            // 註冊所有 Consumers (可選，視需求而定)
            // o.AddConsumer<MyMessageConsumer>(); 也是一種
            // x.AddConsumersFromNamespaceContaining<Program>();
            x.UsingRabbitMq(
                (context, configurator) =>
                {
                    var rabbitMqConfig = configuration.GetSection("RabbitMQ");
                    var hostUrl = rabbitMqConfig["Host"];
                    var username = rabbitMqConfig["Username"] ?? "guest";
                    var password = rabbitMqConfig["Password"] ?? "guest";
                    var virtualHost = rabbitMqConfig["VirtualHost"] ?? "/";
                    var uri = new Uri(hostUrl ?? "");
                    configurator.Host(
                        hostUrl,
                        host =>
                        {
                            host.Username(username);
                            host.Password(password);
                        }
                    );
                    configurator.ConfigureEndpoints(context);
                }
            );
        });

        return services;
    }
}
