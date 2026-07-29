using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Infrastructure.Persistence.Configurations
{
    public class BrokerAccountCredentialConfiguration : IEntityTypeConfiguration<BrokerAccountCredential>
    {
        public void Configure(EntityTypeBuilder<BrokerAccountCredential> builder)
        {
            builder.ToTable("BrokerAccountCredentials");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.AccountId)
                .IsRequired();

            builder.Property(c => c.EncryptedPayload)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(c => c.LastSyncDate)
                .HasColumnType("date");

            builder.HasOne(c => c.Account)
               .WithOne(a => a.BrokerAccountCredentials)
               .HasForeignKey<BrokerAccountCredential>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.AccountId)
                .IsUnique(); // one credential set per broker account, assuming 1:1

        }
    }
}
