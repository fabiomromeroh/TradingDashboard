using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class BrokerRepository : IBrokerRepository
{
    private readonly AppDbContext _context;

    public BrokerRepository(AppDbContext context) => _context = context;

    public async Task<List<Broker>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Brokers.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Brokers.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task AddAsync(Broker broker, CancellationToken cancellationToken)
        => await _context.Brokers.AddAsync(broker, cancellationToken);

    public Task UpdateAsync(Broker broker, CancellationToken cancellationToken)
    {
        _context.Brokers.Update(broker);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Broker broker, CancellationToken cancellationToken)
    {
        _context.Brokers.Remove(broker);
        return Task.CompletedTask;
    }
}
