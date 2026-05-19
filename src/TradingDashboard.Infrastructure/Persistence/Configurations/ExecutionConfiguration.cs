using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class ExecutionConfiguration : IEntityTypeConfiguration<Execution>
{
    public void Configure(EntityTypeBuilder<Execution> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Action);
        builder.Property(e => e.Price).HasColumnType("decimal(18,4)");
        builder.Property(e => e.Quantity).HasColumnType("decimal(18,4)");
        builder.Property(e => e.Notes).HasMaxLength(1000);
        builder.Property(x => x.ExecutedAt).HasColumnType("datetimeoffset");

        // Simple index — enough for ordering and trade-level filtering
        builder.HasIndex(x => x.ExecutedAt);

        builder.HasOne(e => e.Trade)
               .WithMany(t => t.Executions)
               .HasForeignKey(e => e.TradeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
