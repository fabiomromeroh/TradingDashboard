using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct);
        Task<List<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken ct);
        Task AddAsync(RefreshToken token, CancellationToken ct);
        Task UpdateAsync(RefreshToken token, CancellationToken ct);
        Task UpdateRangeAsync(IEnumerable<RefreshToken> tokens, CancellationToken ct);
    }
}
