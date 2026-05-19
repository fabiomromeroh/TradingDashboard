using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class ImportSessionConfiguration : IEntityTypeConfiguration<ImportSession>
{
    public void Configure(EntityTypeBuilder<ImportSession> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.FileName).HasMaxLength(500).IsRequired();
        builder.Property(i => i.ErrorSummary).HasMaxLength(2000);
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);
    }
}
