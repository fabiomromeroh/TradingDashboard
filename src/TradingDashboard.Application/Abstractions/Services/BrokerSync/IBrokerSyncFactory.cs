namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    public interface IBrokerSyncFactory
    {
        IBrokerSyncService GetSyncService(string brokerName);
        IReadOnlyList<string> SupportedBrokers { get; }
    }
}
