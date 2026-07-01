using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class ImportSessionConfiguration : IEntityTypeConfiguration<ImportSession>
{
    public void Configure(EntityTypeBuilder<ImportSession> builder)
    {
        builder.ToTable("ImportSessions");

        builder.HasKey(i => i.Id);

        builder.Property(x => x.BrokerName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(i => i.FileName).HasMaxLength(500).IsRequired(false);

        builder.Property(x => x.FileHash)
            .HasMaxLength(64)      // SHA-256 hex string = 64 chars
            .IsRequired(false);

        builder.Property(x => x.FileFormat)
            .HasMaxLength(10)      // "CSV", "PDF", "XLSX"
            .IsRequired(false);

        builder.Property(x => x.SourceType)
            .IsRequired()
            .HasConversion<string>()   // store as "Manual", "Broker", etc.
            .HasMaxLength(50);

        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);

        builder.Property(x => x.CompletedAt)
            .IsRequired(false);

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.IsRolledBack)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.PeriodStart)
            .IsRequired(false);

        builder.Property(x => x.PeriodEnd)
            .IsRequired(false);

        builder.Property(x => x.TotalRows)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.ProcessedRows)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.SkippedRows)
            .IsRequired()
            .HasDefaultValue(0);

        // Fast lookup of all sessions for a given account
        builder.HasIndex(x => x.AccountId)
            .HasDatabaseName("IX_ImportSessions_AccountId");

        builder.HasOne(x => x.Account)
            .WithMany(x => x.ImportSessions)             // adjust to WithMany(a => a.ImportSessions) if Account exposes the collection
            .HasForeignKey(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Executions)
            .WithOne(e => e.ImportSession)          // adjust to match Execution's nav property name
            .HasForeignKey(e => e.ImportSessionId)  // adjust to match Execution's FK property name
            .OnDelete(DeleteBehavior.Restrict);
    }
}
