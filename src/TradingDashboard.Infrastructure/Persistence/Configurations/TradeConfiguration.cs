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
        builder.Property(t => t.ClosePrice).HasColumnType("decimal(18,4)");

        builder.Property(t => t.Quantity).HasColumnType("decimal(18,4)");


        builder.Property(x => x.OpenedAt).HasColumnType("datetimeoffset");

        // Computed — EF reads it but never writes it
        builder.Property<DateOnly>("OpenedAtDate")
               .HasComputedColumnSql("CAST([OpenedAt] AS DATE)", stored: true);

        builder.Property<int>("OpenedAtHour")
               .HasComputedColumnSql("DATEPART(HOUR, [OpenedAt])", stored: true);

        builder.HasIndex("OpenedAtDate");
        builder.HasIndex("OpenedAtHour");

        builder.Property(x => x.ClosedAt)
               .HasColumnType("datetimeoffset");

        // Computed — EF reads it but never writes it
        builder.Property<DateOnly>("ClosedAtDate")
               .HasComputedColumnSql("CAST([ClosedAt] AS DATE)", stored: true);

        builder.Property<int>("ClosedAtHour")
               .HasComputedColumnSql("DATEPART(HOUR, [ClosedAt])", stored: true);



        builder.HasIndex("ClosedAtDate");
        builder.HasIndex("ClosedAtHour");
        builder.HasIndex(t => t.Symbol);
    }
}
