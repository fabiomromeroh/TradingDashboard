namespace TradingDashboard.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }
}

