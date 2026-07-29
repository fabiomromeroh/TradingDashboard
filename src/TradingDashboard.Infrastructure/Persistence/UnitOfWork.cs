using Microsoft.EntityFrameworkCore.Storage;
using TradingDashboard.Application.Interfaces;

namespace TradingDashboard.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;
        private IDbContextTransaction? dbContextTransaction;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (dbContextTransaction != null)
                throw new InvalidOperationException("Transaction already started.");

            dbContextTransaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (dbContextTransaction == null)
                throw new InvalidOperationException("No active transaction.");

            await appDbContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
            await dbContextTransaction.DisposeAsync();
            dbContextTransaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (dbContextTransaction == null)
                return;

            await dbContextTransaction.RollbackAsync(cancellationToken);
            await dbContextTransaction.DisposeAsync();
            dbContextTransaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
