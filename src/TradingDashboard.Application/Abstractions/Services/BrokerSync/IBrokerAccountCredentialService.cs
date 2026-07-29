using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    public interface IBrokerAccountCredentialService
    {
        Task<TCredentials?> GetAsync<TCredentials>(Guid brokerAccountId, CancellationToken ct) where TCredentials : BrokerCredentials;
        Task CreateAsync(Guid AccountId, string BrokerName, object credentials, CancellationToken ct);
        Task UpdateAsync(BrokerAccountCredential brokerCredentials, object credentials, CancellationToken ct);
    }
}
