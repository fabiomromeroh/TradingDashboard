using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http.Json;
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