using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(rt => rt.ReplacedByTokenHash)
                .HasMaxLength(256);

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            // Unique index — enforces no duplicate hashes and speeds up lookup on refresh/logout [web:37][web:40]
            builder.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            // Index on UserId — used for "logout everywhere" / revoke-all queries [web:35]
            builder.HasIndex(rt => rt.UserId);

            // Foreign key relationship — one User has many RefreshTokens
            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade); // deleting a user cleans up their sessions [web:35]

            builder.Ignore(rt => rt.IsActive); // computed property, not persisted
        }
    }
}
