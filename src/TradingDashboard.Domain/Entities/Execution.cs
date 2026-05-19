using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;
using Action = TradingDashboard.Domain.Enums.Action;

namespace TradingDashboard.Domain.Entities;

public class Execution : BaseEntity
{
    public Action Action { get; set; }
    public InstrumentType Type { get; set; }
   
    public decimal Price { get; private set; }
    public decimal Quantity { get; private set; }
    public TradeDirection Direction { get; private set; }
    public DateTimeOffset ExecutedAt { get; private set; }
    public string? Notes { get; private set; }

    public Guid TradeId { get; private set; }
    public Trade? Trade { get; private set; }

    private Execution() { }

    public static Execution Create(Guid tradeId, decimal price, decimal quantity, TradeDirection direction, string? notes = null)
    {
        return new Execution
        {
            TradeId = tradeId,
            Price = price,
            Quantity = quantity,
            Direction = direction,
            Notes = notes,
            ExecutedAt = DateTime.UtcNow
        };
    }
}
