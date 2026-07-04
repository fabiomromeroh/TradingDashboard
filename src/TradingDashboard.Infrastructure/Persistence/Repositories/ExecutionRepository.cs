using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class ExecutionRepository : IExecutionRepository
{
    private readonly AppDbContext _context;

    public ExecutionRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Execution>> GetAllByTradeIdAsync(Guid tradeId, CancellationToken cancellationToken)
        => await _context.Executions.AsNoTracking().Where(e => e.TradeId == tradeId).OrderBy(x => x.ExecutedAt).ToListAsync(cancellationToken);

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
        return _context.Executions.Where(x => brokerExecutionIds.Contains(x.BrokerExecutionId) && x.AccountId == accountId).Select(x => x.BrokerExecutionId).ToListAsync(ct);
    }

    public async Task<List<Execution>> GetByAccountAndSymbolsAsync(Guid accountId, string[] symbols, CancellationToken ct)
    {
        return await _context.Executions.Where(x => x.AccountId == accountId && symbols.Contains(x.Symbol)).ToListAsync(ct);
    }

    public async Task DeleteRangeAsync(IEnumerable<Execution> executions, CancellationToken ct)
    {
        _context.Executions.RemoveRange(executions);
    }


}
