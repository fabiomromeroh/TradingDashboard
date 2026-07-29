namespace TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr
{
    public class IbkrFlexOptions
    {
        public string BaseUrl { get; set; } = "https://ndcdyn.interactivebrokers.com/AccountManagement/FlexWebService";
        public int MaxAttempts { get; set; } = 8;
        public int InitialDelaySeconds { get; set; } = 5;
        public int PollIntervalSeconds { get; set; } = 5;
    }
}
