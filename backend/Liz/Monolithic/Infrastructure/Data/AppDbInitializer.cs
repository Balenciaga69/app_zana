using Microsoft.EntityFrameworkCore;

namespace Monolithic.Infrastructure.Data
{
    public static class AppDbInitializer
    {
        // 擴充方法：用於遷移資料庫
        public static void MigrateDatabase(this IServiceProvider serviceProvider)
        {
            // 建立一個新的服務範圍
            using var scope = serviceProvider.CreateScope();
            // 從服務範圍中取得 AppDbContext 實例
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // 執行資料庫遷移
            db.Database.Migrate();
        }
    }
}
