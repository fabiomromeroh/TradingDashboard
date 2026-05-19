using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface IImportSessionRepository
{
    Task<IEnumerable<ImportSession>> GetAllByAccountIdAsync(Guid accountId, CancellationToken cancellationToken);
    Task<ImportSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(ImportSession importSession, CancellationToken cancellationToken);
    Task UpdateAsync(ImportSession importSession, CancellationToken cancellationToken);
    Task DeleteAsync(ImportSession importSession, CancellationToken cancellationToken);
}
