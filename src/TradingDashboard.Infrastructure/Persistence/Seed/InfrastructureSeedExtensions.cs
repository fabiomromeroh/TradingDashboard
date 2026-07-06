using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TradingDashboard.Infrastructure.Persistence.Seed
{
    public static class InfrastructureSeedExtensions
    {
        public static void ApplyMigrationsAndSeed(this IServiceProvider services, IConfiguration config)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
            DbSeeder.SeedAdmin(db, config);
        }
    }
}
