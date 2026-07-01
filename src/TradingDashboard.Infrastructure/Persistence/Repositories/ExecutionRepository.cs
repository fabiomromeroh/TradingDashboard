using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class ExecutionRepository : IExecutionRepository
{
    private readonly AppDbContext _context;

    public ExecutionRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Execution>> GetAllByTradeIdAsync(Guid tradeId, CancellationToken cancellationToken)
        => await _context.Executions.AsNoTracking().Where(e => e.TradeId == tradeId).ToListAsync(cancellationToken);

    public async Task<Execution?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Executions.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task AddAsync(Execution execution, CancellationToken cancellationToken)
        => await _context.Executions.AddAsync(execution, cancellationToken);

    public Task UpdateAsync(Execution execution, CancellationToken cancellationToken)
    {
        _context.Executions.Update(execution);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Execution execution, CancellationToken cancellationToken)
    {
        _context.Executions.Remove(execution);
        return Task.CompletedTask;
    }

    public Task<List<string>> GetExistingBrokerExecutionIdsAsync(List<string> brokerExecutionIds, Guid accountId, CancellationToken ct)
    {
        return _context.Executions.Where(x => brokerExecutionIds.Contains(x.BrokerExecutionId)).Select(x => x.BrokerExecutionId).ToListAsync(ct);
    }

}
