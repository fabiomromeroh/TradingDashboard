using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class TotalTradesMetricCalculator : IMetricCalculator
    {
        private readonly ITradeQueryService _queryService;

        public TotalTradesMetricCalculator(ITradeQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "total-trades";

        public string RenderType => "metric";



        public async Task<object> CalculateMetricAsync(Guid userId, ISpecification<Trade> spec, CancellationToken cancellationToken)
        {

            var trades = await _queryService.GetTradesAsync(userId, spec, cancellationToken);

            if (trades == null || !trades.Any())
            {
                return new MetricPayloadDto(
                    DisplayValue: "0",
                    Description: "Total number of trades",
                    Tone: MetricTone.Default,
                    Points: new List<AreaChartPointDto>()
                    );
            }

            List<AreaChartPointDto> points = [];

            DateTime? currentDate = null;
            int cumulative = 0;
            int total = 0;

            foreach (var trade in trades)
            {
                cumulative += 1;
                total += 1;

                if (trade.OpenedAt.Date != currentDate)
                {
                    currentDate = trade.OpenedAt.Date;
                    points.Add(new AreaChartPointDto(trade.OpenedAt.ToString("MMM d, yyyy"), cumulative));
                }

            }

            return new MetricPayloadDto(
                DisplayValue: total.ToString(),
                Description: "Total number of trades",
                Tone: MetricTone.Default,
                Points: points
                );

        }
    }
}
