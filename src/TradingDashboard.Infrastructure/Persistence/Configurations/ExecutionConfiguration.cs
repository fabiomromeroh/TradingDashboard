using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class ExecutionConfiguration : IEntityTypeConfiguration<Execution>
{
    public void Configure(EntityTypeBuilder<Execution> builder)
    {
        builder.ToTable("Executions");

        builder.HasKey(e => e.Id);

        // ── Instrument ───────────────────────────────────────────────────

        builder.Property(x => x.Symbol) // "AAPL", "BTC/USD", "ES=F", etc.
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Side) // "Buy", "Sell"
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(x => x.InstrumentType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);         // "Stock", "Option", "Future", etc.

        // ── Financials ───────────────────────────────────────────────────

        builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 8);      // supports crypto sub-cent prices

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasPrecision(18, 8);

        builder.Property(x => x.Commission)
            .IsRequired()
            .HasPrecision(18, 4)
            .HasDefaultValue(0m);

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasDefaultValue(CurrencyType.USD);

        // ── Broker metadata ──────────────────────────────────────────────

        builder.Property(x => x.Exchange)
            .HasMaxLength(50)
            .IsRequired(false)
            .HasDefaultValue(string.Empty);

        builder.Property(x => x.OrderType)
            .HasMaxLength(30)          // "Market", "Limit", "StopLimit", etc.
            .IsRequired(false)
            .HasDefaultValue(string.Empty);

        builder.Property(x => x.BrokerExecutionId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.BrokerOrderId)
            .HasMaxLength(100)
            .IsRequired(false);


        // ── Timestamp ────────────────────────────────────────────────────

        builder.Property(x => x.ExecutedAt)
            .IsRequired();

        // ── Foreign keys ─────────────────────────────────────────────────

        builder.Property(x => x.TradeId)
            .IsRequired();

        builder.Property(x => x.ImportSessionId)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────────

        // Primary dedup guard — one broker execution ID per import session
        builder.HasIndex(x => new { x.ImportSessionId, x.BrokerExecutionId })
            .IsUnique()
            .HasDatabaseName("UIX_Executions_ImportSessionId_BrokerExecutionId");

        // Lookup all executions belonging to a trade
        builder.HasIndex(x => x.TradeId)
            .HasDatabaseName("IX_Executions_TradeId");

        // Time-range queries per symbol (P&L reports, charts)
        builder.HasIndex(x => new { x.Symbol, x.ExecutedAt })
            .HasDatabaseName("IX_Executions_Symbol_ExecutedAt");


        // ── Relationships ─────────────────────────────────────────────────

        builder.HasOne(x => x.Trade)
            .WithMany(t => t.Executions)
            .HasForeignKey(x => x.TradeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ImportSession)
            .WithMany(s => s.Executions)
            .HasForeignKey(x => x.ImportSessionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
