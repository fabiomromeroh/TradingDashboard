using TradingDashboard.Application.Abstractions.Services.BrokerSync;

namespace TradingDashboard.Infrastructure.Services.BrokerSync
{
    public class BrokerSyncFactory : IBrokerSyncFactory
    {
        private readonly Dictionary<string, IBrokerSyncService> _syncServices;

        public BrokerSyncFactory(IEnumerable<IBrokerSyncService> syncServices)
        {
            _syncServices = syncServices.ToDictionary(p => p.BrokerName, StringComparer.OrdinalIgnoreCase);
        }

        public IBrokerSyncService GetSyncService(string brokerName)
            => _syncServices.TryGetValue(brokerName, out var service)
                ? service
                : throw new NotSupportedException($"No sync service for broker: {brokerName}");

        public IReadOnlyList<string> SupportedBrokers
            => [.. _syncServices.Keys];
    }
}
