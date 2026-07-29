namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    public interface IBrokerSyncService
    {
        string BrokerName { get; }
        Task<BrokerSyncResult> SyncAsync(BrokerSyncRequest request, CancellationToken cancellationToken);
    }

    public record BrokerSyncRequest(BrokerCredentials Credentials, DateOnly DateFrom, DateOnly DateTo);

}
