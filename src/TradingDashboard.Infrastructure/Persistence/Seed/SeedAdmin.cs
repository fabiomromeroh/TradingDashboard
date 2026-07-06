using Microsoft.Extensions.Configuration;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Infrastructure.Persistence.Seed
{
    public static class DbSeeder
    {

        public static void SeedAdmin(AppDbContext db, IConfiguration config)
        {
            if (!db.Users.Any(u => u.Role == UserRole.Admin))
            {
                var adminEmail = config["ADMIN_EMAIL"] ?? "admin@tradingdashboard.local";
                var adminPassword = config["ADMIN_PASSWORD"]
                    ?? throw new InvalidOperationException("ADMIN_PASSWORD must be set");

                User user = User.Create(adminEmail, BCrypt.Net.BCrypt.HashPassword(adminPassword), "Admin", "");
                user.Role = UserRole.Admin;

                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
