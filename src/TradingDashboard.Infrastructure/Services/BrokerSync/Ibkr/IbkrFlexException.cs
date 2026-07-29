namespace TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr
{
    public class IbkrFlexException : Exception
    {
        public string? ErrorCode { get; }
        public IbkrFlexException(string? code, string? message) : base(message) => ErrorCode = code;
        public bool IsRetryable => ErrorCode is not null && IbkrFlexApiClient.RetryableCodes.Contains(ErrorCode);
    }
}
