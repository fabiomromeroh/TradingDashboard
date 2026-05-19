using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Users.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
        => await _context.Users.AddAsync(user, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }
}
