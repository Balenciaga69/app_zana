using Microsoft.EntityFrameworkCore;

namespace Monolithic.Infrastructure.Data
{
    public static class AppDbInitializer
    {
        public static void MigrateDatabase(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }
    }
}
