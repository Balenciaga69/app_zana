// RabbitMQ 相關 ServiceCollection 擴充
namespace Liz.Monolithic.Infrastructure.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static void AddRabbitMqOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));
        }
    }

    public class RabbitMQOptions
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? VirtualHost { get; set; }
    }
}
