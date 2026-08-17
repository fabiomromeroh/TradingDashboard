using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class WinRateMetricCalculator : IMetricCalculator
    {
        private readonly IMetricQueryService _queryService;

        public WinRateMetricCalculator(IMetricQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "win-rate";
        public string RenderType => "gauge";

        public async Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await _queryService.GetTradeAggregatesAsync(spec, cancellationToken);
            var percent = trades is null || trades.TotalTrades == 0 ? 0 : (decimal)trades.WinningTrades / trades.TotalTrades * 100;

            return new GaugePayloadDto(
                DisplayValue: $"{percent:F0}%",
                "Percentage of trades that are winners.",
                Percent: percent,
                Stats:
                [
                new("Wins", trades.WinningTrades.ToString(), MetricTone.Success),
                new("Losses", trades.LosingTrades.ToString(), MetricTone.Danger)
                ]);
        }
    }
}
