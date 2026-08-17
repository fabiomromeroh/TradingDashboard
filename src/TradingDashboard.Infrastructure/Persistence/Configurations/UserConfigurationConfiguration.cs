using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace TradingDashboard.Infrastructure.Persistence.Configurations;

public class UserConfigurationConfiguration : IEntityTypeConfiguration<Domain.Entities.UserConfiguration>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.UserConfiguration> builder)
    {
        builder.ToTable("UserConfigurations");

        builder.HasKey(uc => uc.Id);

        // ────── Scalar properties ─────────────────────────────────────────

        builder.Property(uc => uc.FiltersJson)
            .IsRequired()
            .HasDefaultValue("{}")
            .HasColumnType("nvarchar(max)");

        builder.Property(uc => uc.WidgetLayoutJson)
            .IsRequired()
            .HasDefaultValue("[]")
            .HasColumnType("nvarchar(max)");

        // ────── Foreign keys ──────────────────────────────────────────────

        builder.Property(uc => uc.UserId)
            .IsRequired();

        // ────── Relationships ────────────────────────────────────────────

        builder.HasOne(uc => uc.User)
            .WithOne()
            .HasForeignKey<Domain.Entities.UserConfiguration>(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ────── Indexes ──────────────────────────────────────────────────

        builder.HasIndex(uc => uc.UserId)
            .IsUnique();
    }
}
