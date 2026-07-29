using TradingDashboard.Application.Abstractions.Services.BrokerSync;

namespace TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr
{
    public record IbkrFlexCredentials(string QueryId, string Token) : BrokerCredentials;

}
