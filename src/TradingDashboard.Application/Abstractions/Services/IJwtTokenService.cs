using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
