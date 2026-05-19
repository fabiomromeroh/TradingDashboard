using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(150).IsRequired();
        builder.Property(a => a.Currency).HasMaxLength(10).IsRequired();
        builder.Property(a => a.InitialBalance).HasColumnType("decimal(18,4)");

        builder.HasOne(a => a.Broker)
               .WithMany(b => b.Accounts)
               .HasForeignKey(a => a.BrokerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Trades)
               .WithOne(t => t.Account)
               .HasForeignKey(t => t.AccountId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.ImportSessions)
               .WithOne(i => i.Account)
               .HasForeignKey(i => i.AccountId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
