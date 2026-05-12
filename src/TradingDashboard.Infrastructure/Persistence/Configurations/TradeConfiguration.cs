using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Symbol).HasMaxLength(20).IsRequired();
        builder.Property(t => t.EntryPrice).HasColumnType("decimal(18,4)");
        builder.Property(t => t.Quantity).HasColumnType("decimal(18,4)");
        builder.HasIndex(t => t.Symbol);
    }
}
