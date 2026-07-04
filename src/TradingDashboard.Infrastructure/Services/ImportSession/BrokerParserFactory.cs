using TradingDashboard.Application.Services.Import.Interfaces;

namespace TradingDashboard.Infrastructure.Services.Import
{
    public class BrokerParserFactory : IBrokerParserFactory
    {
        private readonly Dictionary<string, IBrokerParser> _parsers;

        public BrokerParserFactory(IEnumerable<IBrokerParser> parsers)
        {
            _parsers = parsers.ToDictionary(p => p.BrokerName, StringComparer.OrdinalIgnoreCase);
        }

        public IBrokerParser GetParser(string brokerName)
            => _parsers.TryGetValue(brokerName, out var parser)
                ? parser
                : throw new NotSupportedException($"No parser for broker: {brokerName}");

        public IReadOnlyList<string> SupportedBrokers
            => [.. _parsers.Keys];
    }
}
