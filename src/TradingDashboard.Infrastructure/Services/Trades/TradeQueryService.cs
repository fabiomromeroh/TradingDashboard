using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Common.Models;
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

        public async Task<PaginatedResult<Trade>> GetTradesPaginatedAsync(Guid userId, ISpecification<Trade> spec, int pageSize, string? cursor, CancellationToken ct)
        {
            IQueryable<Trade> baseQuery = appDbContext
                                            .Trades
                                            .AsNoTracking()
                                            .Where(t => t.Account.UserId == userId)
                                            .Where(spec.ToExpression());

            int totalCount = await baseQuery.CountAsync(ct);


            // Apply cursor filter for pagination
            IQueryable<Trade> query = baseQuery;
            if (!string.IsNullOrEmpty(cursor))
            {
                var (openedAt, tradeId) = DecodeCursor(cursor);
                query = query.Where(t =>
                    t.OpenedAt < openedAt ||
                    (t.OpenedAt == openedAt && t.Id > tradeId)
                );
            }

            // Apply ordering and fetch
            var trades = await query
                .OrderByDescending(x => x.OpenedAt)
                .ThenBy(x => x.Id)
                .Take(pageSize + 1)
                .ToListAsync(ct);

            var hasMore = trades.Count > pageSize;
            var items = trades.Take(pageSize).ToList();

            string? nextCursor = null;
            if (hasMore && items.Count > 0)
            {
                var lastItem = items[^1];
                nextCursor = EncodeCursor(lastItem.OpenedAt, lastItem.Id);
            }

            return new PaginatedResult<Trade>(items, nextCursor, hasMore, totalCount);
        }

        private static string EncodeCursor(DateTimeOffset openedAt, Guid tradeId)
        {
            var cursorData = $"{openedAt:O}|{tradeId}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(cursorData);
            return Convert.ToBase64String(bytes);
        }

        private static (DateTimeOffset openedAt, Guid tradeId) DecodeCursor(string cursor)
        {
            try
            {
                var bytes = Convert.FromBase64String(cursor);
                var cursorData = System.Text.Encoding.UTF8.GetString(bytes);
                var parts = cursorData.Split('|');

                if (parts.Length != 2 ||
                    !DateTimeOffset.TryParse(parts[0], out var openedAt) ||
                    !Guid.TryParse(parts[1], out var tradeId))
                {
                    throw new ArgumentException("Invalid cursor format");
                }

                return (openedAt, tradeId);
            }
            catch
            {
                throw new ArgumentException("Invalid or corrupted cursor");
            }
        }
    }
}
