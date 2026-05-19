using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
