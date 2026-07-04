namespace TradingDashboard.Application.Features.Trades.Dtos
{
    public record ExecutionDto(
        Guid Id,
        string Symbol,
        string Side,
        string InstrumentType,
        decimal Price,
        decimal Quantity,
        decimal Commission,
        string? OrderType,
        DateTimeOffset ExecutedAt,
        Guid TradeId
        );

}
