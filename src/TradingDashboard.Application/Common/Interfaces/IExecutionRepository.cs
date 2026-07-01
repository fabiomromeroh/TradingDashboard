using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface IExecutionRepository
{
    Task<IEnumerable<Execution>> GetAllByTradeIdAsync(Guid tradeId, CancellationToken cancellationToken);
    Task<Execution?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Execution execution, CancellationToken cancellationToken);
    Task UpdateAsync(Execution execution, CancellationToken cancellationToken);
    Task DeleteAsync(Execution execution, CancellationToken cancellationToken);
    Task<List<string>> GetExistingBrokerExecutionIdsAsync(List<string> brokerExecutionIds, Guid accountId, CancellationToken ct);
}
