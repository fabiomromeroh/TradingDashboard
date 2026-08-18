using Microsoft.EntityFrameworkCore;
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

        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> operation,
            CancellationToken cancellationToken = default)
        {
            var strategy = appDbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await operation(cancellationToken);
                    await appDbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default)
        {
            var strategy = appDbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await operation(cancellationToken);
                    await appDbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
    }

}
