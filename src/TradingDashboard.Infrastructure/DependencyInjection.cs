using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Infrastructure.Identity;
using TradingDashboard.Infrastructure.Persistence;
using TradingDashboard.Infrastructure.Persistence.Repositories;

namespace TradingDashboard.Infrastructure;

/// <summary>
/// JWT Settings configuration class for dependency injection
/// </summary>
public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "TradingDashboard";
    public string Audience { get; set; } = "TradingDashboard";
    public int ExpiryMinutes { get; set; } = 60;
}

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- JWT Configuration ---
        // Binds environment variables to strongly-typed JwtSettings
        // Supports both appsettings.json and App Service environment variables
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // --- Database ---
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                    .MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // --- Repositories ---
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IBrokerRepository, BrokerRepository>();
        services.AddScoped<IImportSessionRepository, ImportSessionRepository>();
        services.AddScoped<IExecutionRepository, ExecutionRepository>();

        // --- Unit of Work ---
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // --- Identity ---
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}

