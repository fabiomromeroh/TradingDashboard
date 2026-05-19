using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class Trade : BaseEntity
{
    public string Symbol { get; private set; } = string.Empty;
    public decimal EntryPrice { get; private set; }
    public decimal ClosePrice { get; set; }
    public decimal Quantity { get; private set; }
    public TradeDirection Direction { get; private set; }
    public TradeStatus Status { get; private set; } = TradeStatus.Open;
    public DateTimeOffset OpenedAt { get; private set; }
    public DateTimeOffset ClosedAt { get; set; }

    public Guid AccountId { get; private set; }
    public Account? Account { get; private set; }

    public IReadOnlyCollection<Execution> Executions => _executions.AsReadOnly();
    private readonly List<Execution> _executions = [];

    // EF Core constructor
    private Trade() { }

    public static Trade Create(string symbol, decimal entryPrice, decimal quantity, TradeDirection direction, Guid accountId = default)
    {
        return new Trade
        {
            Symbol = symbol,
            EntryPrice = entryPrice,
            Quantity = quantity,
            Direction = direction,
            OpenedAt = DateTime.UtcNow,
            AccountId = accountId
        };
    }
}
