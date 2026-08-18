using System.Globalization;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class NetPnlMetricCalculator : IMetricCalculator
    {
        private readonly ITradeQueryService _queryService;

        public NetPnlMetricCalculator(ITradeQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "net-pnl";

        public string RenderType => "metric";


        public async Task<object> CalculateMetricAsync(Guid userId, ISpecification<Trade> spec, CancellationToken cancellationToken)
        {

            var trades = await _queryService.GetTradesAsync(userId, spec, cancellationToken);

            if (trades is null || !trades.Any())
            {
                return new MetricPayloadDto(
                    DisplayValue: "$0.00",
                    Description: "Total accumulated net profit and loss.",
                    Tone: MetricTone.Default,
                    Points: []
                );
            }

            List<AreaChartPointDto> points = [];

            DateTime? currentDate = null;
            decimal cumulative = 0m;

            foreach (var trade in trades)
            {
                cumulative += trade.NetReturnOrZero;

                if (trade.OpenedAt.Date != currentDate)
                {
                    currentDate = trade.OpenedAt.Date;
                    points.Add(new AreaChartPointDto(trade.OpenedAt.ToString("MMM d, yyyy"), Math.Round(cumulative, 2)));
                }

            }

            return new MetricPayloadDto(
                DisplayValue: $"${Math.Round(cumulative, 2, MidpointRounding.AwayFromZero).ToString("N2", CultureInfo.InvariantCulture)}",
                Description: "Total accumulated net profit and loss.",
                Tone: cumulative >= 0 ? MetricTone.Success : MetricTone.Danger,
                points

                );

        }
    }
}
