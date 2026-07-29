using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Repositories;

public interface IExecutionRepository
{
    Task<IEnumerable<Execution>> GetAllByTradeIdAsync(Guid tradeId, CancellationToken cancellationToken);
    Task<Execution?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Execution execution, CancellationToken cancellationToken);
    Task UpdateAsync(Execution execution, CancellationToken cancellationToken);
    Task DeleteAsync(Execution execution, CancellationToken cancellationToken);
    Task<List<string>> GetExistingBrokerExecutionIdsAsync(List<string> brokerExecutionIds, Guid accountId, CancellationToken ct);
    Task<List<Execution>> GetByAccountAndSymbolsAsync(Guid accountId, string[] symbols, CancellationToken ct);

    Task DeleteRangeAsync(IEnumerable<Execution> executions, CancellationToken ct);
}
