using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class Trade : BaseEntity
{
    public  string Symbol { get; private set; }
    public decimal EntryPrice { get; private set; }
    public decimal Quantity { get; private set; }
    public TradeDirection Direction { get; private set; }
    public TradeStatus Status { get; private set; } = TradeStatus.Open;
    public DateTime OpenedAt { get; private set; }

    // EF Core constructor
    private Trade() { }

    public static Trade Create(string symbol, decimal entryPrice, decimal quantity, TradeDirection direction)
    {
        return new Trade
        {
            Symbol = symbol,
            EntryPrice = entryPrice,
            Quantity = quantity,
            Direction = direction,
            OpenedAt = DateTime.UtcNow
        };
    }
}
