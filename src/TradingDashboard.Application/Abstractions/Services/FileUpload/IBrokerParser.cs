using TradingDashboard.Application.Abstractions.Services.FileUpload.Models;

namespace TradingDashboard.Application.Abstractions.Services.Import
{
    public interface IBrokerParser
    {
        string BrokerName { get; }
        ParsedImportResult Parse(byte[] fileBytes);
    }
}
