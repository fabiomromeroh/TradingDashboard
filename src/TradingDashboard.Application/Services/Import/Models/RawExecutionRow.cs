namespace TradingDashboard.Application.Services.Import.Models
{
    public record RawExecutionRow(
        int RowNumber,
        string BrokerExecutionId,   // ExecID  — your duplicate detection key
        string BrokerOrderId,       // OrderID — groups partial fills of one order
        string BrokerTradeId,       // TradeID — groups related orders (scale-in/out)
        string Symbol,
        string Description,
        string AssetClass,          // "STK", "ADR" — future options support
        string Currency,            // "USD" — future multi-currency
        string Side,                // "Buy" / "Sell" — normalized
        decimal Quantity,            // always positive
        decimal Price,
        decimal Commission,          // always positive, total commission
        string Exchange,            // "DARK", "NASDAQ", "IBKRATS"
        string OrderType,           // "LMT" / "MKT"
        DateTimeOffset ExecutedAt           // UTC
    );

}
