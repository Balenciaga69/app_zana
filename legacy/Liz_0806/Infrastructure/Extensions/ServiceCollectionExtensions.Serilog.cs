using Serilog;
using Serilog.Enrichers.Sensitive;

namespace Monolithic.Infrastructure.Extensions;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 配置 Serilog 日誌服務
    /// </summary>
    public static void AddSerilogLogging(this WebApplicationBuilder builder)
    {
        // 敏感資料欄位，會被遮罩
        var sensitiveDataKeys = new string[]
        {
            "Password",
            "Token",
            "Secret",
            "AccessToken",
            "ApiKey",
            "ClientSecret",
            "Key",
            "AuthorizationCode",
            "RefreshToken",
            "SessionId",
            "Cookie",
            "Authorization",
        };

        // 過長資料欄位，會被遮罩
        var tooLongDataKeys = new string[]
        {
            "AvatarUrl",
            "Url",
            "Picture",
            "Image",
            "Photo",
            "Content",
            "Description",
            "Body",
            "Html",
            "Xml",
            "Json",
        };
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithSensitiveDataMasking(opts =>
            {
                opts.MaskValue = "***MASKED***";
                opts.Mode = MaskingMode.Globally;
                opts.MaskProperties.AddRange(sensitiveDataKeys);
            })
            .Enrich.WithSensitiveDataMasking(opts =>
            {
                opts.MaskValue = "***TooLong***";
                opts.Mode = MaskingMode.Globally;
                opts.MaskProperties.AddRange(tooLongDataKeys);
            })
            .CreateLogger();

        builder.Host.UseSerilog();

        // 註冊 Serilog 為主要日誌提供者
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}
