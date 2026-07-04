using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class TradeRepository : ITradeRepository
{
    private readonly AppDbContext context;

    public TradeRepository(AppDbContext appDbContext)
    {
        this.context = appDbContext;
    }

    public async Task AddTradeAsync(Trade trade, CancellationToken cancellationToken)
    {

        await context.Trades.AddAsync(trade, cancellationToken);

    }
    public async Task<Trade> FindOrCreateTradeAsync(PreviewRowDto row, Guid accountId, CancellationToken cancellationToken)
    {
        // Look for an open trade on the same symbol for this account
        var trade = await context.Trades

            .FirstOrDefaultAsync(x => x.AccountId == accountId && x.Status == Domain.Enums.TradeStatus.Open && x.Symbol == row.Symbol, cancellationToken);

        if (trade is not null)
            return trade;

        // No open trade — this execution opens a new one
        var direction = row.Side == "Buy" ? TradeDirection.Long : TradeDirection.Short;

        var newTrade = Trade.Create(

            symbol: row.Symbol,
            entryPrice: row.Price,
            quantity: row.Quantity,
            direction: direction,
            accountId: accountId,
            openedAt: row.ExecutedAt);

        await context.Trades.AddAsync(newTrade, cancellationToken);

        return newTrade;
    }


    public async Task DeleteTradeAsync(Trade trade, CancellationToken cancellationToken)
    {
        context.Trades.Remove(trade);
    }

    public async Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Trades.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    }

    public async Task<IEnumerable<Trade>> GetTradesAsync(CancellationToken cancellationToken)
    {
        return await context.Trades.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Trade>> GetOpenTradesByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
    {
        return await context.Trades.Where(x => x.AccountId == accountId && x.Status == TradeStatus.Open).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trade>> GetTradesByAccountId(List<Guid> accountIds, CancellationToken cancellationToken)
    {
        return await context.Trades.Where(x => accountIds.Contains(x.AccountId)).ToListAsync(cancellationToken);
    }

    public Task AddTradeRangeAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken)
    {
        return context.Trades.AddRangeAsync(trades, cancellationToken);
    }

    public void RemoveTradeRangeByAccountAndSymbol(Guid accountId, string[] symbols)
    {
        var trades = context.Trades
        .Where(t => t.AccountId == accountId && symbols.Contains(t.Symbol))
        .ToList();

        context.Trades.RemoveRange(trades);
    }
}
