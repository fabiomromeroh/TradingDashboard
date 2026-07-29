using TradingDashboard.Domain.Common;

namespace TradingDashboard.Domain.Entities
{
    public class BrokerAccountCredential : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string EncryptedPayload { get; set; } = default!; // JSON, encrypted as opaque blob
        public DateOnly? LastSyncDate { get; set; }

        public Account Account { get; set; } = default!; // navigation property

        public static BrokerAccountCredential Create(Guid accountId, string encryptedPayload)
        {
            return new BrokerAccountCredential
            {
                AccountId = accountId,
                EncryptedPayload = encryptedPayload,
                LastSyncDate = null

            };
        }

        public BrokerAccountCredential UpdateEncryptedPayload(string encryptedPayload)
        {
            EncryptedPayload = encryptedPayload;
            UpdatedAt = DateTime.UtcNow;
            return this;
        }

        public void UpdateLastSyncDate(DateOnly lastSyncDate)
        {
            LastSyncDate = lastSyncDate;
        }
    }
}
