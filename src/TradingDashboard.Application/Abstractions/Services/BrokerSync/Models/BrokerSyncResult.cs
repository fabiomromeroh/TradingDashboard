namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    public class BrokerSyncResult
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<ParsedExecution> Executions { get; }
        public string? ErrorCode { get; }
        public string? ErrorMessage { get; }

        private BrokerSyncResult(bool isSuccess, IReadOnlyList<ParsedExecution> executions, string? errorCode, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Executions = executions;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public static BrokerSyncResult Success(IReadOnlyList<ParsedExecution> executions) =>
            new(true, executions, null, null);

        public static BrokerSyncResult Failure(string errorCode, string errorMessage) =>
            new(false, [], errorCode, errorMessage);
    }
}
