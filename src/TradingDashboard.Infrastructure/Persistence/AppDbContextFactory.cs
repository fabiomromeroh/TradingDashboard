//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace TradingDashboard.Infrastructure.Persistence;

//public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//{
//    public AppDbContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//        optionsBuilder.UseSqlServer(
//            "Server=localhost,1433;Database=TradingDashboardDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;Encrypt=False;",
//            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

//        return new AppDbContext(optionsBuilder.Options);
//    }
//}
