using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Infrastructure.Persistence;

namespace TradingDashboard.Infrastructure.Services.Metric
{
    public class MetricQueryService : IMetricQueryService
    {
        private readonly AppDbContext appDbContext;

        public MetricQueryService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(QueryFilter filter, CancellationToken ct)
        {
            var result = await appDbContext.Trades
                .Where(x => x.Account.IsActive && filter.AccountIds.Contains(x.AccountId))
                .GroupBy(x => 1)
                .Select(x => new DashboardSummaryDto
                {
                    NetPnl = x.Sum(x => x.NetReturn ?? 0),
                    TradeCount = x.Count(),
                    WinCount = x.Count(x => x.NetReturn > 0),
                    LossCount = x.Count(x => x.NetReturn < 0),
                    AverageWin = x.Where(x => x.NetReturn > 0).Average(x => x.NetReturn ?? 0),
                    AverageLoss = x.Where(x => x.NetReturn < 0).Average(x => x.NetReturn ?? 0)

                }).FirstOrDefaultAsync(ct);

            return result ?? new DashboardSummaryDto();
        }

        public async Task<TradeAggregateDto> GetTradeAggregatesAsync(ISpecification<Trade> spec, CancellationToken ct)
        {
            var query = appDbContext
                .Trades
                .Where(spec.ToExpression());

            return await query
                .GroupBy(_ => 1)
                .Select(g => new TradeAggregateDto(
                    TotalTrades: g.Count(),
                    WinningTrades: g.Count(t => t.NetReturn > 0),
                    LosingTrades: g.Count(t => t.NetReturn <= 0),
                    AvgWin: g.Where(t => t.NetReturn > 0).Average(t => t.NetReturn ?? 0),
                    AvgLoss: g.Where(t => t.NetReturn < 0).Average(t => t.NetReturn ?? 0),
                    GrossProfit: g.Where(t => t.NetReturn > 0).Sum(t => t.NetReturn ?? 0),
                    GrossLoss: g.Where(t => t.NetReturn <= 0).Sum(t => t.NetReturn ?? 0),
                    NetPnl: g.Sum(t => t.NetReturn ?? 0)))
                .FirstOrDefaultAsync(ct) ?? new TradeAggregateDto(0, 0, 0, 0, 0, 0, 0, 0);
        }

        public async Task<IEnumerable<Trade>> GetTradesAsync(ISpecification<Trade> spec, CancellationToken ct)
        {
            return await appDbContext.Trades
                .Where(spec.ToExpression())
                .OrderBy(x => x.OpenedAt)
                .ToListAsync(ct);


        }
    }
}
