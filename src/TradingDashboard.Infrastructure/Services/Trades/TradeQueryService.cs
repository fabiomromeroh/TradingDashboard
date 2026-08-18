using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Infrastructure.Persistence;

namespace TradingDashboard.Infrastructure.Services.Trades
{
    public class TradeQueryService(AppDbContext appDbContext) : ITradeQueryService
    {
        public async Task<TradeAggregateDto> GetTradeAggregatesAsync(Guid userId, ISpecification<Trade> spec, CancellationToken ct)
        {
            var query = appDbContext
                .Trades
                .Where(t => t.Account.UserId == userId)
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

        public async Task<IEnumerable<Trade>> GetTradesAsync(Guid userId, ISpecification<Trade> spec, CancellationToken ct)
        {
            return await appDbContext.Trades
                .Where(t => t.Account.UserId == userId)
                .Where(spec.ToExpression())
                .OrderBy(x => x.OpenedAt)
                .ToListAsync(ct);


        }

        public async Task<IEnumerable<Trade>> GetTradesByAccountId(List<Guid> accountIds, CancellationToken cancellationToken)
        {
            return await appDbContext.Trades
                .AsNoTracking()
                .Where(x => accountIds.Contains(x.AccountId))
                .ToListAsync(cancellationToken);
        }

        public async Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await appDbContext.Trades.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        }


    }
}
