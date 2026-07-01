using TradingDashboard.Domain.Common;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Domain.Entities;

public class Execution : BaseEntity
{
    public required string Symbol { get; set; }
    public Side Side { get; private set; }
    public InstrumentType InstrumentType { get; private set; }

    public decimal Price { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Commission { get; private set; }
    public CurrencyType Currency { get; private set; } = CurrencyType.USD;
    public string? Exchange { get; private set; } = string.Empty;
    public string? OrderType { get; private set; } = string.Empty;
    public DateTimeOffset ExecutedAt { get; private set; }

    public Guid TradeId { get; private set; }
    public required string BrokerExecutionId { get; set; }
    public string? BrokerOrderId { get; private set; }

    public Guid ImportSessionId { get; set; }
    public Trade? Trade { get; private set; }
    public ImportSession ImportSession { get; set; } = null!;

    private Execution() { }

    public static Execution Create(Guid tradeId, string symbol, decimal price, decimal quantity, Side side, DateTimeOffset executedAt, decimal commission, string brokerExecutionId, string brokerOrderId, Guid importSessionId, string? exchange = default, string? orderType = default, CurrencyType currency = default)
    {
        return new()
        {
            Symbol = symbol,
            TradeId = tradeId,
            BrokerExecutionId = brokerExecutionId,
            BrokerOrderId = brokerOrderId,
            Price = price,
            Quantity = quantity,
            Side = side,
            ExecutedAt = executedAt,
            ImportSessionId = importSessionId,
            Currency = currency,
            Commission = commission,
        };
    }
}
