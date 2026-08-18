using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class DailyPnlMetricCalculator(ITradeQueryService metricQueryService) : IMetricCalculator
    {
        public string MetricType => "daily-pnl-bar";

        public string RenderType => "bar-chart";

        public async Task<object> CalculateMetricAsync(Guid userId, ISpecification<Trade> spec, CancellationToken cancellationToken)
        {

            var trades = await metricQueryService.GetTradesAsync(userId, spec, cancellationToken);

            if (trades is null || !trades.Any())
            {
                return new BarChartPayloadDto("Daily PnL", []);
            }

            var result = trades
                .GroupBy(x => x.OpenedAt.Date)
                .OrderBy(x => x.Key)
                .Select(x => new AreaChartPointDto
            (
                X: x.First().OpenedAt.ToString("MMM d, yyyy"),
                Y: x.Sum(t => t.NetReturnOrZero)

            )).ToList();

            return new BarChartPayloadDto("Daily PnL", result);
        }
    }
}
