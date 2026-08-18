using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class TradeRepository(AppDbContext appDbContext) : ITradeRepository
{
    public async Task AddTradeAsync(Trade trade, CancellationToken cancellationToken)
    {

        await appDbContext.Trades.AddAsync(trade, cancellationToken);

    }

    public async Task DeleteTradeAsync(Trade trade, CancellationToken cancellationToken)
    {
        appDbContext.Trades.Remove(trade);
    }

    public Task AddTradeRangeAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken)
    {
        return appDbContext.Trades.AddRangeAsync(trades, cancellationToken);
    }

    public void RemoveTradeRangeByAccountAndSymbol(Guid accountId, string[] symbols)
    {
        var trades = appDbContext.Trades
        .Where(t => t.AccountId == accountId && symbols.Contains(t.Symbol))
        .ToList();

        appDbContext.Trades.RemoveRange(trades);
    }
}
