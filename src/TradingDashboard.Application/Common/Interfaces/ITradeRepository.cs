using System.Collections;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface ITradeRepository
{
    public Task<IEnumerable<Trade>> GetTradesAsync(CancellationToken cancellationToken);
    public Task AddTradeAsync(Trade trade, CancellationToken cancellationToken);
    public Task<Trade?> GetTradeAsync(Guid id, CancellationToken cancellationToken);
}
