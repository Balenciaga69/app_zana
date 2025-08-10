using Microsoft.EntityFrameworkCore;
using Monolithic.Infrastructure.Data;

namespace Monolithic.Infrastructure.Extensions;

public static partial class DIExt
{
    // 將 PostgreSQL DbContext 加入 DI 容器
    public static void AddPostgresDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // 取得連線字串，若未設定則拋出例外
        var connectionString =
            configuration.GetConnectionString("UserDbConnection")
            ?? throw new InvalidOperationException("PostgreSQL connection string not found.");

        // 註冊 AppDbContext，並設定使用 Npgsql (PostgreSQL) 提供者
        services.AddDbContext<AppDbContext>(options =>
        {
            // 設定 Npgsql 相關選項
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    // 啟用失敗重試，最多重試 3 次
                    npgsqlOptions.EnableRetryOnFailure(3);
                    // 設定指令逾時為 30 秒
                    npgsqlOptions.CommandTimeout(30);
                }
            );

            // 預設查詢不追蹤，提升查詢效能
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }
}
