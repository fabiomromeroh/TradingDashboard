using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Services.Metric.Calculators
{
    public class NetPnlCurveCalculator : IMetricCalculator
    {
        private readonly ITradeQueryService _queryService;

        public NetPnlCurveCalculator(ITradeQueryService queryService)
        {
            _queryService = queryService;
        }
        public string MetricType => "net-pnl-curve";

        public string RenderType => "area-chart";


        public async Task<object> CalculateMetricAsync(Guid userId, ISpecification<Trade> spec, CancellationToken cancellationToken)
        {
            var trades = await _queryService.GetTradesAsync(userId, spec, cancellationToken);

            if (trades is null || !trades.Any())
            {
                return new AreaChartPayloadDto("Net PnL Curve", new List<AreaChartPointDto>());
            }

            List<AreaChartPointDto> points = new List<AreaChartPointDto>();

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

            return new AreaChartPayloadDto("Net PnL Curve", points);
        }
    }
}
