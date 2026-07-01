using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.ToTable("Trades");

        builder.HasKey(t => t.Id);

        // ── Instrument ───────────────────────────────────────────────────

        builder.Property(x => x.Symbol)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Direction)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);         // "Long", "Short"

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(TradeStatus.Open);

        // ── Financials ───────────────────────────────────────────────────

        builder.Property(x => x.EntryPrice)
            .IsRequired()
            .HasPrecision(18, 8);

        builder.Property(x => x.AverageEntryPrice)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasDefaultValue(0m);

        builder.Property(x => x.ClosePrice)
            .IsRequired(false)         // null until trade closes
            .HasPrecision(18, 8);

        builder.Property(x => x.AverageClosePrice)
            .IsRequired(false)         // null until trade closes; VWAP across all closing executions
            .HasPrecision(18, 8);

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasPrecision(18, 8);

        builder.Property(x => x.PositionSize)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasDefaultValue(0m);

        builder.Property(x => x.TotalCommissions)
            .IsRequired()
            .HasPrecision(18, 8)
            .HasDefaultValue(0m);

        builder.Property(x => x.NetReturn)
            .IsRequired(false)         // null until trade closes
            .HasPrecision(18, 8);

        builder.Property(x => x.PercentageReturn)
            .IsRequired(false)         // null until trade closes
            .HasPrecision(10, 4);      // e.g. 12.3456%

        // ── Timestamps ───────────────────────────────────────────────────

        builder.Property(x => x.OpenedAt)
            .IsRequired();

        builder.Property(x => x.ClosedAt)
            .IsRequired(false);        // null until RecalculatePosition() closes it

        // ── Foreign keys ─────────────────────────────────────────────────

        builder.Property(x => x.AccountId)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────────

        // All trades for an account — the most common query in the dashboard
        builder.HasIndex(x => x.AccountId)
            .HasDatabaseName("IX_Trades_AccountId");

        // Open trades filter — used by FindOrCreateTradeAsync to match incoming executions
        builder.HasIndex(x => new { x.AccountId, x.Symbol, x.Status })
            .HasDatabaseName("IX_Trades_AccountId_Symbol_Status");

        // Date-range P&L reports
        builder.HasIndex(x => new { x.AccountId, x.OpenedAt })
            .HasDatabaseName("IX_Trades_AccountId_OpenedAt");

        // ── Relationships ─────────────────────────────────────────────────

        builder.HasOne(x => x.Account)
            .WithMany(a => a.Trades)    // adjust to match Account's collection name
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Executions)
            .WithOne(e => e.Trade)
            .HasForeignKey(e => e.TradeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Executions)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_executions");
    }
}
