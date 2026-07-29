namespace TradingDashboard.Application.Abstractions.Services.Import
{
    public interface IBrokerParserFactory
    {
        IBrokerParser GetParser(string brokerName);
        IReadOnlyList<string> SupportedBrokers { get; }
    }
}
