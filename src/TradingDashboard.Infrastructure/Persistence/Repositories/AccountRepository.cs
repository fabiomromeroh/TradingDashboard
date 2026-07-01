using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
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
        .Where(a => a.UserId == userId)
        .Select(x => mapper.Map<AccountDto>(x))
        .ToListAsync(cancellationToken);

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task AddAsync(Account account, CancellationToken cancellationToken)
        => await _context.Accounts.AddAsync(account, cancellationToken);

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
