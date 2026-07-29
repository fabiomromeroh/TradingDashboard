using System.Xml.Linq;

namespace TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr
{
    public interface IIbkrFlexApiClient
    {
        Task<string> RequestReportAsync(string queryId, string token, string dateFrom, string dateTo, CancellationToken ct);
        Task<XDocument> GetReportAsync(string referenceCode, string token, CancellationToken ct);
    }
}
