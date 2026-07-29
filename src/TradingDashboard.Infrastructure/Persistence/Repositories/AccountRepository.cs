using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper mapper;

    public AccountRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => await _context.Accounts.AsNoTracking()
        .Include(x => x.Broker)
        .Include(x => x.BrokerAccountCredentials)
        .Where(a => a.UserId == userId && a.IsActive)
        .Select(x => mapper.Map<AccountDto>(x))
        .ToListAsync(cancellationToken);

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Accounts
        .Include(x => x.Broker)
        .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);

    public async Task<Guid> AddAsync(Account account, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.Accounts.AddAsync(account, cancellationToken);
        return entityEntry.Entity.Id;
    }


    public Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        _context.Accounts.Update(account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Account account, CancellationToken cancellationToken)
    {
        _context.Accounts.Remove(account);
        return Task.CompletedTask;
    }
}
