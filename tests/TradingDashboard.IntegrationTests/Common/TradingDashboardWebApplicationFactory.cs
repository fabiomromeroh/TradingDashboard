using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TradingDashboard.Infrastructure.Persistence;


namespace TradingDashboard.IntegrationTests.Common
{
    public class TradingDashboardWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {

                // Remove any existing provider configuration for AppDbContext
                var providerConfigDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbContextOptionsConfiguration<AppDbContext>));

                if (providerConfigDescriptor is not null)
                {
                    services.Remove(providerConfigDescriptor);
                }

                // Optional: also remove DbContextOptions<AppDbContext> if you have it
                var optionsDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (optionsDescriptor is not null)
                {
                    services.Remove(optionsDescriptor);
                }

                // Replace the real database context with an in-memory database for testing
                services.AddDbContext<AppDbContext>(options =>
                {
                    TestDbContext.ConfigureForTests(options);
                });

                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                TestDbContext.SeedTestData(db);
            });
        }
    }
}
