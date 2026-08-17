using TradingDashboard.Domain.Common;

namespace TradingDashboard.Domain.Entities
{
    public class UserConfiguration : BaseEntity
    {
        public Guid UserId { get; set; }

        public string FiltersJson { get; set; } = "{}"; // date range, symbol, tags — flexible, low-integrity-risk fields
        public string WidgetLayoutJson { get; set; } = "[]"; // ordered widget keys + visibility

        public User User { get; set; } = null!;
    }
}
