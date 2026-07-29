using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Repositories
{
    public interface IBrokerAccountCredentialRepository
    {
        Task<BrokerAccountCredential?> GetAsync(Guid accountId, CancellationToken ct);
        Task UpdateAsync(BrokerAccountCredential brokerAccountCredential, CancellationToken ct);
        Task AddAsync(BrokerAccountCredential brokerAccountCredential, CancellationToken ct);
    }
}
