namespace TradingDashboard.Application.Abstractions.Services.BrokerSync
{
    public record ParsedExecution
    {
        public int RowNumber { get; init; }
        public string BrokerExecutionId { get; init; } = default!;
        public string BrokerOrderId { get; init; } = default!;
        public string BrokerTradeId { get; init; } = default!;
        public string? TransactionId { get; init; }
        public string Symbol { get; init; } = default!;
        public string? UnderlyingSymbol { get; init; }
        public string Description { get; init; } = default!;
        public string AssetClass { get; init; } = default!;
        public string Currency { get; init; } = default!;
        public string Side { get; init; } = default!;
        public decimal Quantity { get; init; }
        public decimal Price { get; init; }
        public decimal Commission { get; init; }
        public string? CommissionCurrency { get; init; }
        public decimal? Proceeds { get; init; }
        public decimal? NetCash { get; init; }
        public string? Exchange { get; init; }
        public string? OrderType { get; init; }
        public string? OpenCloseIndicator { get; init; }
        public decimal? RealizedPnl { get; init; }
        public decimal? Strike { get; init; }
        public DateOnly? Expiry { get; init; }
        public string? PutCall { get; init; }
        public decimal? Multiplier { get; init; }
        public DateTimeOffset ExecutedAt { get; init; }
        public string BrokerName { get; init; } = default!;
        public string SourceType { get; init; } = default!;
    }
}
