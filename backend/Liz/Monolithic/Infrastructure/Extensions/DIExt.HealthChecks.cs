using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
{
    /// <summary>
    /// 註冊應用程式健康檢查服務
    /// </summary>
    /// <param name="services">服務集合</param>
    /// <param name="configuration">設定</param>
    public static void AddAppHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var healthChecksBuilder = services.AddHealthChecks();

        // PostgreSQL 健康檢查
        var postgresConnectionString = configuration.GetConnectionString("UserDbConnection");
        if (!string.IsNullOrEmpty(postgresConnectionString))
        {
            healthChecksBuilder.AddNpgSql(
                postgresConnectionString,
                name: "postgresql",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "postgresql" }
            );
        }

        // Redis 健康檢查
        var redisConnectionString = configuration.GetConnectionString("UserRedis");
        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            healthChecksBuilder.AddRedis(
                redisConnectionString,
                name: "redis",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "cache", "redis" }
            );
        }

        // RabbitMQ 健康檢查
        var rabbitMqConfig = configuration.GetSection("RabbitMQ");
        var rabbitMqHost = rabbitMqConfig["Host"];
        string? rabbitMqConnectionString = null;

        // 若 Host 已經是完整 URI，直接用
        if (!string.IsNullOrWhiteSpace(rabbitMqHost) && rabbitMqHost.StartsWith("amqp://"))
        {
            rabbitMqConnectionString = rabbitMqHost;
        }
        else if (!string.IsNullOrWhiteSpace(rabbitMqHost))
        {
            var rabbitMqPort = rabbitMqConfig.GetValue<int>("Port", 5672);
            var rabbitMqUsername = rabbitMqConfig["Username"] ?? "guest";
            var rabbitMqPassword = rabbitMqConfig["Password"] ?? "guest";
            var rabbitMqVirtualHost = rabbitMqConfig["VirtualHost"] ?? "/";
            rabbitMqConnectionString = $"amqp://{rabbitMqUsername}:{rabbitMqPassword}@{rabbitMqHost}:{rabbitMqPort}{rabbitMqVirtualHost}";
        }

        if (!string.IsNullOrWhiteSpace(rabbitMqConnectionString))
        {
            healthChecksBuilder.AddRabbitMQ(
                rabbitMqConnectionString,
                name: "rabbitmq",
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "messaging", "rabbitmq" }
            );
        }
    }
}
