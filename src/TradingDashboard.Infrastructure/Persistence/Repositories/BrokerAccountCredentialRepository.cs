using Microsoft.EntityFrameworkCore;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Repositories
{
    public class BrokerAccountCredentialRepository : IBrokerAccountCredentialRepository
    {
        private readonly AppDbContext context;

        public BrokerAccountCredentialRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(BrokerAccountCredential brokerAccountCredential, CancellationToken ct)
        {
            await context.BrokerAccountCredentials.AddAsync(brokerAccountCredential, ct);
        }

        public async Task<BrokerAccountCredential?> GetAsync(Guid accountId, CancellationToken ct)
        {
            return await context.BrokerAccountCredentials.FirstOrDefaultAsync(b => b.AccountId == accountId, ct);
        }

        public async Task UpdateAsync(BrokerAccountCredential brokerAccountCredential, CancellationToken ct)
        {
            context.BrokerAccountCredentials.Update(brokerAccountCredential);
        }


    }
}
