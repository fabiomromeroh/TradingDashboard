using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        // ── Scalar properties ────────────────────────────────────────────

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired(false)
            .HasDefaultValue("USD");

        builder.Property(x => x.InitialBalance)
            .IsRequired()
            .HasPrecision(18, 4)
            .HasDefaultValue(0m);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.ImportSourceType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ImportSourceType.BrokerSync);

        // ── Foreign keys ─────────────────────────────────────────────────

        builder.Property(x => x.UserId)
            .IsRequired();


        builder.Property(x => x.BrokerId)
            .IsRequired();

        // ── Indexes ──────────────────────────────────────────────────────

        // All accounts for a user — most common query
        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("IX_Accounts_UserId");

        // Unique account name per user — no duplicate "Main Account" names
        builder.HasIndex(x => new { x.UserId, x.Name })
            .IsUnique()
            .HasDatabaseName("UIX_Accounts_UserId_Name");

        // Filter active accounts quickly (soft-delete pattern)
        builder.HasIndex(x => new { x.UserId, x.IsActive })
            .HasDatabaseName("IX_Accounts_UserId_IsActive");

        // ── Relationships ─────────────────────────────────────────────────

        builder.HasOne(x => x.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Broker)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.BrokerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Trades)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Trades)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_trades");

        builder.HasMany(x => x.ImportSessions)
            .WithOne(s => s.Account)
            .HasForeignKey(s => s.AccountId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Navigation(x => x.ImportSessions)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_importSessions");
    }
}
