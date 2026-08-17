using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Infrastructure.Persistence;
using TradingDashboard.IntegrationTests.Common;

namespace TradingDashboard.IntegrationTests;

[Collection("IntegrationTestsCollection")]
public class TradesApiTests
{
    private readonly TradingDashboardWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TradesApiTests(TradingDashboardWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTrades_ReturnsOkResponse()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/trades");
        // Act
        var response = await _client.SendAsync(request);
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetTrades_WithAuthenticationForSpecificUserAndAccount_ReturnTrades()
    {
        // Arrange: Get a specific user and their account from the seeded in-memory database
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Get the first user (Alice) and her first account (Alice-IBKR-USD)
        var user = db.Users.FirstOrDefault(u => u.Email == "alice@test.com");
        user.Should().NotBeNull();

        var userAccount = db.Accounts.FirstOrDefault(a => a.UserId == user.Id && a.Name == "Alice-IBKR-USD");
        userAccount.Should().NotBeNull();

        // Get trades for this specific account
        var expectedTrades = db.Trades.Where(t => t.AccountId == userAccount.Id).ToList();
        expectedTrades.Should().NotBeEmpty();

        // Generate a valid JWT token for the authenticated user
        var token = TokenHelper.GenerateTokenForUser(_factory, user);
        token.Should().NotBeNullOrEmpty();

        // Create an authenticated HTTP client with the token
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/trades/accounts")
        {
            Content = JsonContent.Create(new List<Guid> { userAccount.Id })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act: Get trades for the specific account with authentication
        var response = await _client.SendAsync(request);

        // Assert: HTTP response is successful
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert: Trades are returned and match expected data
        var trades = await response.Content.ReadFromJsonAsync<List<TradeDto>>();
        trades.Should().NotBeNull();
        trades!.Should().NotBeEmpty();
        trades.Should().HaveCount(expectedTrades.Count);

        // Assert: Trades belong to the correct account and user
        var symbols = trades.Select(t => t.Symbol).ToList();
        var expectedSymbols = expectedTrades.Select(t => t.Symbol).ToList();
        symbols.Should().BeEquivalentTo(expectedSymbols);

        // Assert: domain-level expectations - verify specific symbols from seeded data
        // Alice's IBKR account has AAPL and META trades
        symbols.Should().Contain(["AAPL", "META"]);
    }

    [Fact]
    public async Task GetTrades_RequiresAuthentication()
    {
        // Arrange: Prepare to call the trades endpoint without authentication
        using var scope = _factory.Services.CreateScope();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        // Act: Call the trades endpoint without authorization header
        var response = await _client.PostAsync("/api/trades/accounts",
            JsonContent.Create(new List<Guid> { Guid.NewGuid() }));

        if (!environment.IsDevelopment())
        {
            // Assert: Should return Unauthorized
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

}