using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr;

namespace TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr
{
    public class IbkrSyncService : IBrokerSyncService
    {
        private readonly IIbkrFlexApiClient _client;
        private readonly IbkrFlexReportParser _parser;
        private readonly ILogger<IbkrSyncService> _logger;
        private readonly IbkrFlexOptions _options;

        public IbkrSyncService(IIbkrFlexApiClient client, IbkrFlexReportParser parser, ILogger<IbkrSyncService> logger, IOptions<IbkrFlexOptions> options)
        {
            _client = client;
            _parser = parser;
            _logger = logger;
            _options = options.Value;
        }

        public string BrokerName => "Interactive Brokers";

        public async Task<BrokerSyncResult> SyncAsync(BrokerSyncRequest request, CancellationToken ct)
        {
            string referenceCode;
            IbkrFlexCredentials credentials = (IbkrFlexCredentials)request.Credentials;

            if (string.IsNullOrEmpty(credentials.Token) || string.IsNullOrEmpty(credentials.QueryId))
                return BrokerSyncResult.Failure("EmptyCredentials", "Token or QueryId is empty.");

            try
            {
                referenceCode = await _client.RequestReportAsync(credentials.QueryId, credentials.Token, request.DateFrom.ToString("yyyyMMdd"), request.DateTo.ToString("yyyyMMdd"), ct);
            }
            catch (IbkrFlexException ex)
            {
                _logger.LogError("Flex request failed: {Code} {Message}", ex.ErrorCode, ex.Message);
                return BrokerSyncResult.Failure(ex.ErrorCode ?? "REQUEST_FAILED", ex.Message);
            }

            _logger.LogInformation("Flex report requested, ReferenceCode={Ref}", referenceCode);
            await Task.Delay(TimeSpan.FromSeconds(_options.InitialDelaySeconds), ct);

            for (var attempt = 1; attempt <= _options.MaxAttempts; attempt++)
            {
                try
                {
                    var xml = await _client.GetReportAsync(referenceCode, credentials.Token, ct);
                    var executions = _parser.Parse(xml);
                    return BrokerSyncResult.Success(executions);
                }
                catch (IbkrFlexException ex) when (ex.IsRetryable && attempt < _options.MaxAttempts)
                {
                    _logger.LogWarning("Report not ready (attempt {Attempt}): {Message}", attempt, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(_options.PollIntervalSeconds) * attempt, ct);
                }
                catch (IbkrFlexException ex)
                {
                    _logger.LogError("Non-retryable Flex error: {Code} {Message}", ex.ErrorCode, ex.Message);
                    return BrokerSyncResult.Failure(ex.ErrorCode ?? "UNKNOWN", ex.Message);
                }
                catch (IbkrFlexParseException ex)
                {
                    _logger.LogError("Failed to parse Flex report: {Message}", ex.Message);
                    return BrokerSyncResult.Failure("PARSE_ERROR", ex.Message);
                }
            }

            return BrokerSyncResult.Failure("TIMEOUT", "Report was not ready after max polling attempts.");
        }

    }
}
