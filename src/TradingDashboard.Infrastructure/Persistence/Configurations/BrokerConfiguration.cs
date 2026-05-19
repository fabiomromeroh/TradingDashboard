using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class BrokerConfiguration : IEntityTypeConfiguration<Broker>
{
    public void Configure(EntityTypeBuilder<Broker> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        builder.Property(b => b.DisplayName).HasMaxLength(150).IsRequired();
        builder.Property(b => b.Website).HasMaxLength(250);
        builder.Property(b => b.SupportedImportFormat).HasMaxLength(100);
        builder.HasIndex(b => b.Name).IsUnique();
    }
}
