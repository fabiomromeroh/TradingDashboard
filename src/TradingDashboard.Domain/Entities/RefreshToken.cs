using TradingDashboard.Domain.Common;

namespace TradingDashboard.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTimeOffset ExpiresAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }
        public string? ReplacedByTokenHash { get; private set; } // for rotation chain
        public User User { get; private set; } = default!;
        public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;

        public static RefreshToken Create(Guid userId, string tokenHash)
        {
            return new RefreshToken()
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(60) // sliding window
            };
        }

        public void Replace(string token)
        {
            ReplacedByTokenHash = token;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            if (RevokedAt is not null)
                throw new InvalidOperationException("Token is already revoked.");

            RevokedAt = DateTime.UtcNow;

        }
    }
}
