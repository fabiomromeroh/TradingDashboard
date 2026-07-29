using Microsoft.Extensions.DependencyInjection;
using TradingDashboard.Application.Abstractions.Services;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.IntegrationTests.Common;

public static class TokenHelper
{
    /// <summary>
    /// Generates a valid JWT token for a user for testing purposes
    /// </summary>
    public static string GenerateTokenForUser(TradingDashboardWebApplicationFactory factory, User user)
    {
        using var scope = factory.Services.CreateScope();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        return jwtTokenService.GenerateToken(user);
    }
}
