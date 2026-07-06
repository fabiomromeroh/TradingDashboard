using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    private static readonly Guid InteractiveBrokersId = Guid.Parse("c3a2b8d9-5f1a-4b6d-9f2e-1a2b3c4d5e6f");
    private static readonly DateTimeOffset SeedCreatedAt =
        new DateTime(2026, 6, 22, 9, 38, 56, 536, DateTimeKind.Utc).AddTicks(9367);

    public void Configure(EntityTypeBuilder<Broker> builder)
    {

        builder.ToTable("Brokers");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        builder.Property(b => b.DisplayName).HasMaxLength(150).IsRequired();
        builder.Property(b => b.Website).HasMaxLength(250);
        builder.Property(b => b.SupportedImportFormat).HasMaxLength(100);
        builder.HasIndex(b => b.Name).IsUnique();

        builder.HasMany(x => x.Accounts)
            .WithOne(x => x.Broker)
            .HasForeignKey(x => x.BrokerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new
            {
                Id = InteractiveBrokersId,
                Name = "Interactive Brokers",
                DisplayName = "IBKR",
                Website = "https://www.interactivebrokers.com",
                SupportedImportFormat = (string?)null,
                CreatedAt = SeedCreatedAt
            }

        );
    }
}
