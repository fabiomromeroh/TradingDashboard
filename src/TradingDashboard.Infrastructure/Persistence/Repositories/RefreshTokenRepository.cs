using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository(AppDbContext appDbContext) : IRefreshTokenRepository
    {
        private readonly AppDbContext _context = appDbContext;

        public async Task AddAsync(RefreshToken token, CancellationToken ct)
        {
            await _context.RefreshTokens.AddAsync(token, ct);
        }

        public async Task<List<RefreshToken>> GetActiveByUserIdAsync(Guid userId, CancellationToken ct)
        {
            return await _context.RefreshTokens.Where(x => x.UserId == userId && x.IsActive).ToListAsync(ct);

        }

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
        }


        public async Task UpdateAsync(RefreshToken token, CancellationToken ct)
        {
            _context.Update(token);
        }

        public async Task UpdateRangeAsync(IEnumerable<RefreshToken> tokens, CancellationToken ct)
        {
            _context.UpdateRange(tokens);
        }
    }
}
