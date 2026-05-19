using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

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

    public async Task DeleteTradeAsync(Trade trade, CancellationToken cancellationToken)
    {
        context.Trades.Remove(trade);
    }

    public async Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken)
    {
        return  await context.Trades.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

    }

    public async Task<IEnumerable<Trade>> GetTradesAsync(CancellationToken cancellationToken)
    {
        return await context.Trades.ToListAsync(cancellationToken: cancellationToken);
    }
}
