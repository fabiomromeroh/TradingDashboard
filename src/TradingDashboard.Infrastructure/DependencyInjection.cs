using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Infrastructure.Persistence;
using TradingDashboard.Infrastructure.Persistence.Repositories;
using TradingDashboard.Infrastructure.Services.BrokerSync;
using TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr;
using TradingDashboard.Infrastructure.Services.Identity;
using TradingDashboard.Infrastructure.Services.Import;
using TradingDashboard.Infrastructure.Services.Import.Ibkr;
using TradingDashboard.Infrastructure.Services.Metric;
using TradingDashboard.Infrastructure.Services.Metric.Calculators;

namespace TradingDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // --- Options Configuration ---
        services.Configure<JwtSettingsOptions>(configuration.GetSection("JwtSettings"));
        services.Configure<IbkrFlexOptions>(configuration.GetSection("IbkrFlex"));


        // --- Database ---
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)));


        // --- Repositories ---
        services.AddScoped<ITradeRepository, TradeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IBrokerRepository, BrokerRepository>();
        services.AddScoped<IImportSessionRepository, ImportSessionRepository>();
        services.AddScoped<IExecutionRepository, ExecutionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IBrokerAccountCredentialRepository, BrokerAccountCredentialRepository>();

        //---Query Services---
        services.AddScoped<IMetricQueryService, MetricQueryService>();

        //---Metric Calculator Factory---
        services.AddScoped<IMetricCalculatorFactory, MetricCalculatorFactory>();
        services.AddScoped<IMetricCalculator, WinRateMetricCalculator>();
        services.AddScoped<IMetricCalculator, NetPnlMetricCalculator>();
        services.AddScoped<IMetricCalculator, TotalTradesMetricCalculator>();
        services.AddScoped<IMetricCalculator, AverageWinLossMetricCalculator>();
        services.AddScoped<IMetricCalculator, ProfitFactorMetricCalculator>();
        services.AddScoped<IMetricCalculator, NetPnlCurveCalculator>();
        services.AddScoped<IMetricCalculator, DayWinRateMetricCalculator>();
        services.AddScoped<IMetricCalculator, DailyPnlMetricCalculator>();
        services.AddScoped<IMetricCalculator, MonthlyPnlMetricCalculator>();



        // --- Unit of Work ---
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // --- Identity ---
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        ////---Import---
        services.AddScoped<IBrokerParser, IbkrCsvParser>();
        services.AddScoped<IBrokerParserFactory, BrokerParserFactory>();
        services.AddScoped<IImportService, ImportSessionService>();

        ////--- Broker Sync ---
        services.AddHttpClient<IIbkrFlexApiClient, IbkrFlexApiClient>()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        services.AddScoped<IBrokerSyncFactory, BrokerSyncFactory>();
        services.AddScoped<IBrokerSyncService, IbkrSyncService>();
        services.AddScoped<IBrokerAccountCredentialService, BrokerAccountCredentialService>();
        services.AddScoped<IbkrFlexReportParser>();
        services.AddScoped<HttpClient>();

        return services;
    }
}

