using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TradingDashboard.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;

        // Log BEFORE handler runs — what command/query came in
        _logger.LogInformation("Handling {RequestName}", requestName);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await next();   // run the actual handler

            stopwatch.Stop();

            // Log AFTER success — how long it took
            _logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            // Log on failure — what command failed and why
            _logger.LogError(ex, "Error handling {RequestName} after {ElapsedMs}ms",
                requestName, stopwatch.ElapsedMilliseconds);

            throw;   // ← re-throws so ExceptionHandlingMiddleware still catches it
        }
    }
}
