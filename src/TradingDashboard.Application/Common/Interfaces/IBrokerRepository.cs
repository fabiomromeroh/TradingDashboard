using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Common.Interfaces;

public interface IBrokerRepository
{
    Task<IEnumerable<Broker>> GetAllAsync(CancellationToken cancellationToken);
    Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Broker broker, CancellationToken cancellationToken);
    Task UpdateAsync(Broker broker, CancellationToken cancellationToken);
    Task DeleteAsync(Broker broker, CancellationToken cancellationToken);
}
