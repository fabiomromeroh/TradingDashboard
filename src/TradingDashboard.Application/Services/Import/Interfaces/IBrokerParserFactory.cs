namespace TradingDashboard.Application.Services.Import.Interfaces
{
    public interface IBrokerParserFactory
    {
        IBrokerParser GetParser(string brokerName);
        IReadOnlyList<string> SupportedBrokers { get; }
    }
}
