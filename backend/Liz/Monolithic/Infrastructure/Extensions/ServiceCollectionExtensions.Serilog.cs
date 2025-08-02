// Serilog 相關 ServiceCollection 擴充
using Serilog;

namespace Liz.Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static void AddSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
        builder.Host.UseSerilog();
    }
}
