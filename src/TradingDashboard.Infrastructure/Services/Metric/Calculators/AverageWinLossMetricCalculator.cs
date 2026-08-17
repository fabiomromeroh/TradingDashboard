using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class AverageWinLossMetricCalculator : IMetricCalculator
    {
        private readonly IMetricQueryService _queryService;

        public AverageWinLossMetricCalculator(IMetricQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "avg-win-loss";

        public string RenderType => "range";

        public async Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await _queryService.GetTradeAggregatesAsync(spec, cancellationToken);

            if (trades is null || trades.TotalTrades == 0)
            {
                return new RangePayloadDto("0", "No trades found.", new StatDto("Avg. Win", "0"), new StatDto("Avg. Loss", "0"), 0);
            }

            var averageWin = trades.WinningTrades > 0 ? Math.Abs(Math.Round(trades.AvgWin / trades.AvgLoss, 2)) : 0;
            var avgWin = Math.Round(trades.AvgWin, 2).ToString();
            var avgLoss = Math.Round(trades.AvgLoss, 2).ToString();
            var ratio = Math.Round(trades.AvgWin / (trades.AvgWin + Math.Abs(trades.AvgLoss)), 2);

            return new RangePayloadDto(averageWin.ToString(), "Average win divided by average loss.", new StatDto("Avg. Win", avgWin), new StatDto("Avg. Loss", avgLoss), ratio);
        }
    }
}