namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊 Redis 服務
    /// </summary>
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("UserRedis");

        // 註冊分散式快取
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        return services;
    }
}
