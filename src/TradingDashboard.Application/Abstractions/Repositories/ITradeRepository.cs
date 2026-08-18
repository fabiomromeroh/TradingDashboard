using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Repositories;

public interface ITradeRepository
{
    public Task AddTradeAsync(Trade trade, CancellationToken cancellationToken);
    public Task DeleteTradeAsync(Trade trade, CancellationToken cancellationToken);
    public Task AddTradeRangeAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken);
    void RemoveTradeRangeByAccountAndSymbol(Guid accountId, string[] symbols);
}
