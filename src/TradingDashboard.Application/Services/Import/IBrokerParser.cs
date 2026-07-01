using TradingDashboard.Application.Services.Import.Models;

namespace TradingDashboard.Application.Services.Import
{
    public interface IBrokerParser
    {
        string BrokerName { get; }
        ParsedImportResult Parse(byte[] fileBytes);
    }
}
