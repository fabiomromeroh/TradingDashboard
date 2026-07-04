using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface ITradeRepository
{
    public Task<IEnumerable<Trade>> GetTradesAsync(CancellationToken cancellationToken);
    public Task<IEnumerable<Trade>> GetOpenTradesByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);

    public Task AddTradeAsync(Trade trade, CancellationToken cancellationToken);
    public Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken);
    public Task DeleteTradeAsync(Trade id, CancellationToken cancellationToken);

    public Task<Trade> FindOrCreateTradeAsync(PreviewRowDto row, Guid accountId, CancellationToken cancellationToken);
    public Task<IEnumerable<Trade>> GetTradesByAccountId(List<Guid> accountIds, CancellationToken cancellationToken);
    public Task AddTradeRangeAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken);
    void RemoveTradeRangeByAccountAndSymbol(Guid accountId, string[] symbols);
}
