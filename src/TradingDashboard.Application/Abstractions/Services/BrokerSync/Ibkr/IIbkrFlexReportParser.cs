using System.Xml.Linq;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;

namespace TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr
{
    public interface IIbkrFlexReportParser
    {
        IReadOnlyList<ParsedExecution> Parse(XDocument report);
    }
}
