using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class DayWinRateMetricCalculator : IMetricCalculator
    {
        private readonly IMetricQueryService _queryService;

        public DayWinRateMetricCalculator(IMetricQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "day-win-rate";

        public string RenderType => "gauge";

        public async Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await _queryService.GetTradesAsync(spec, cancellationToken);

            if (trades is null || !trades.Any())
            {
                return new GaugePayloadDto(
                    DisplayValue: "0%",
                    Description: "Percentage of days that are winners.",
                    Percent: 0,
                    Stats: []
                );
            }

            var groupedByDay = trades.GroupBy(t => t.OpenedAt.Date);

            int winningDays = groupedByDay.Count(g => g.Sum(t => t.NetReturnOrZero) > 0);
            int totalDays = groupedByDay.Count();

            decimal winRate = (decimal)winningDays / totalDays * 100;

            return new GaugePayloadDto(
                DisplayValue: $"{Math.Round(winRate, 2)}%",
                Description: "Percentage of days that are winners.",
                Percent: winRate,
                Stats: [
                    new("Wins", winningDays.ToString(), MetricTone.Success),
                    new("Losses", (totalDays - winningDays).ToString(), MetricTone.Danger)
                ]
            );
        }
    }
}
