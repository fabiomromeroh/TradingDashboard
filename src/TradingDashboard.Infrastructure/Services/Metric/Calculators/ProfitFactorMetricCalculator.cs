using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class ProfitFactorMetricCalculator : IMetricCalculator
    {
        private readonly IMetricQueryService _queryService;

        public ProfitFactorMetricCalculator(IMetricQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "profit-factor";

        public string RenderType => "range";

        public async Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await _queryService.GetTradeAggregatesAsync(spec, cancellationToken);

            if (trades is null || trades.TotalTrades == 0)
            {
                return new RangePayloadDto("0", "Gross profit divided by gross loss.", new StatDto("Gross Profit", "0"), new StatDto("Gross Loss", "0"), 0);
            }

            var profitFactor = trades.GrossLoss == 0 ? 0 : Math.Abs(trades.GrossProfit / trades.GrossLoss);
            var grossProfit = Math.Round(trades.GrossProfit, 2).ToString();
            var grossLoss = Math.Round(trades.GrossLoss, 2).ToString();
            var ratio = Math.Round(trades.GrossProfit / (trades.GrossProfit + Math.Abs(trades.GrossLoss)), 2);


            return new RangePayloadDto(Math.Round(profitFactor, 2).ToString(),
                "Gross profit divided by gross loss.",
                new StatDto("Gross Profit", grossProfit),
                new StatDto("Gross Loss", grossLoss)
            , ratio);

        }
    }
}
