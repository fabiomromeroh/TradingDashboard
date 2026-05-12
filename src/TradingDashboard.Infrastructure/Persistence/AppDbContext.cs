using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Trade> Trades => Set<Trade>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}