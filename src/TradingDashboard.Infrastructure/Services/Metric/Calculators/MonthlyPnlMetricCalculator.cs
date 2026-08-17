using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class MonthlyPnlMetricCalculator : IMetricCalculator
    {
        private readonly IMetricQueryService _metricQueryService;

        public MonthlyPnlMetricCalculator(IMetricQueryService metricQueryService)
        {
            _metricQueryService = metricQueryService;
        }
        public string MetricType => "monthly-pnl";

        public string RenderType => "bar-chart";

        public async Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken)
        {

            var trades = await _metricQueryService.GetTradesAsync(spec, cancellationToken);

            if (trades is null || !trades.Any())
            {
                return new BarChartPayloadDto("Monthly PnL", new List<AreaChartPointDto>());
            }

            var result = trades
                .GroupBy(x => new { x.OpenedAt.Year, x.OpenedAt.Month })
                .OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month)
                .Select(x => new AreaChartPointDto
            (
                X: x.First().OpenedAt.ToString("MMM yyyy"),
                Y: x.Sum(t => t.NetReturnOrZero)

            )).ToList();

            return new BarChartPayloadDto("Monthly PnL", result);
        }
    }
}
