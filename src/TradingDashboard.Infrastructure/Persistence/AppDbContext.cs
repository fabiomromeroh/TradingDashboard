using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trade> Trades => Set<Trade>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Broker> Brokers => Set<Broker>();
    public DbSet<ImportSession> ImportSessions => Set<ImportSession>();
    public DbSet<Execution> Executions => Set<Execution>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<BrokerAccountCredential> BrokerAccountCredentials => Set<BrokerAccountCredential>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}