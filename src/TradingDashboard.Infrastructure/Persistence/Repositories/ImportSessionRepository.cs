using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class ImportSessionRepository : IImportSessionRepository
{
    private readonly AppDbContext _context;

    public ImportSessionRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ImportSession>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken)
        => await _context.ImportSessions.AsNoTracking().Where(i => i.AccountId == accountId).ToListAsync(cancellationToken);

    public async Task<ImportSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.ImportSessions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task AddAsync(ImportSession importSession, CancellationToken cancellationToken)
        => await _context.ImportSessions.AddAsync(importSession, cancellationToken);

    public Task UpdateAsync(ImportSession importSession, CancellationToken cancellationToken)
    {
        _context.ImportSessions.Update(importSession);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ImportSession importSession, CancellationToken cancellationToken)
    {
        _context.ImportSessions.Remove(importSession);
        return Task.CompletedTask;
    }
}
