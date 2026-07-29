using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Repositories;

public interface IBrokerRepository
{
    Task<List<Broker>> GetAllAsync(CancellationToken cancellationToken);
    Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Broker broker, CancellationToken cancellationToken);
    Task UpdateAsync(Broker broker, CancellationToken cancellationToken);
    Task DeleteAsync(Broker broker, CancellationToken cancellationToken);
}
