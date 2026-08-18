using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class WinRateMetricCalculator(ITradeQueryService queryService) : IMetricCalculator
    {
        public string MetricType => "win-rate";
        public string RenderType => "gauge";

        public async Task<object> CalculateMetricAsync(Guid userId, ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await queryService.GetTradeAggregatesAsync(userId, spec, cancellationToken);
            var percent = trades is null || trades.TotalTrades == 0 ? 0 : (decimal)trades.WinningTrades / trades.TotalTrades * 100;
            var wins = trades?.WinningTrades ?? 0;
            var losses = trades?.LosingTrades ?? 0;


            return new GaugePayloadDto(
                DisplayValue: $"{percent:F0}%",
                "Percentage of trades that are winners.",
                Percent: percent,
                Stats:
                [
                new("Wins", wins.ToString(), MetricTone.Success),
                new("Losses", losses.ToString(), MetricTone.Danger)
                ]);
        }
    }
}
