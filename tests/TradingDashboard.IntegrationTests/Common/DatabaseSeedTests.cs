using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TradingDashboard.Infrastructure.Persistence;

namespace TradingDashboard.IntegrationTests.Common;


[Collection("IntegrationTestsCollection")]
public class DatabaseSeedTests
{
    private readonly TradingDashboardWebApplicationFactory _factory;

    public DatabaseSeedTests(TradingDashboardWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void InMemoryDatabase_IsSeeded_WithUsersAccountsTradesExecutions()
    {
        // Arrange: get a scoped AppDbContext from the factory
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Act: query the seeded data
        var users = db.Users.ToList();
        var brokers = db.Brokers.ToList();
        var accounts = db.Accounts.ToList();
        var importSessions = db.ImportSessions.ToList();
        var trades = db.Trades.ToList();
        var executions = db.Executions.ToList();

        // Assert: basic counts (adjust if you change the seed)
        users.Should().HaveCount(3);
        brokers.Should().HaveCount(3);
        accounts.Should().HaveCount(4);
        importSessions.Should().NotBeEmpty();
        trades.Should().HaveCount(5);

        // Every trade should have at least one execution with the same symbol
        foreach (var trade in trades)
        {
            var relatedExecutions = executions
                .Where(e => e.AccountId == trade.AccountId && e.Symbol == trade.Symbol)
                .ToList();

            relatedExecutions.Should().NotBeEmpty($"trade {trade.Symbol} should have executions with same symbol");
        }

        // Optional: assert on specific known symbols from the seed
        trades.Should().Contain(t => t.Symbol == "AAPL");
        trades.Should().Contain(t => t.Symbol == "MSFT");
        trades.Should().Contain(t => t.Symbol == "TSLA");
        trades.Should().Contain(t => t.Symbol == "NVDA");
        trades.Should().Contain(t => t.Symbol == "META");
    }

    [Fact]
    public void InMemoryDatabase_ContainsExpectedSeedEntities()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Act
        var users = db.Users.ToList();
        var accounts = db.Accounts.ToList();
        var trades = db.Trades.ToList();
        var executions = db.Executions.ToList();
        var importSessions = db.ImportSessions.ToList();

        // Assert: basic non-empty checks
        users.Should().NotBeEmpty();
        accounts.Should().NotBeEmpty();
        trades.Should().NotBeEmpty();
        executions.Should().NotBeEmpty();
        importSessions.Should().NotBeEmpty();

        // Assert: specific seed markers (emails & symbols you created in TestDbContext.SeedTestData)
        users.Select(u => u.Email).Should().Contain(new[]
        {
            "alice@test.com",
            "bob@test.com",
            "charlie@test.com"
        });

        trades.Select(t => t.Symbol).Should().Contain(new[]
        {
            "AAPL", "MSFT", "TSLA", "NVDA", "META"
        });

        // Assert: each trade has at least one matching execution (same accountId + symbol)
        foreach (var trade in trades)
        {
            var relatedExecutions = executions
                .Where(e => e.AccountId == trade.AccountId && e.Symbol == trade.Symbol)
                .ToList();

            relatedExecutions.Should().NotBeEmpty($"trade {trade.Symbol} should have executions with same symbol");
        }
    }
}