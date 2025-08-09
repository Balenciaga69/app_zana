namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 註冊 Communication 服務相關服務 (SignalR, 即時通訊)
    /// </summary>
    public static IServiceCollection AddCommunicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // SignalR 核心服務
        services.AddSignalR(options =>
        {
            // 開發環境啟用詳細錯誤資訊
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            options.EnableDetailedErrors = isDevelopment;

            // 設定客戶端逾時
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
            // 設定握手逾時
            options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            // 設定心跳間隔
            options.KeepAliveInterval = TimeSpan.FromSeconds(30);
            // 訊息大小限制 (32KB，適合純文字聊天)
            options.MaximumReceiveMessageSize = 32 * 1024;
        });

        // TODO: @Copilot (可選)未來加入 Redis Backplane 支援水平擴展
        // 需要安裝: Microsoft.AspNetCore.SignalR.StackExchangeRedis
        // signalRBuilder.AddStackExchangeRedis(connectionString);
        return services;
    }
}
