namespace TradingDashboard.Application.Features.ImportSessions.Dtos
{
    public record PreviewRowDto(
        int RowNumber,
        string Symbol,
        string Description,
        string Side,
        decimal Quantity,
        decimal Price,
        decimal Commission,
        string Exchange,
        string OrderType,
        DateTimeOffset ExecutedAt,
        bool IsDuplicate,
        string? ParseError,      // null if row is valid
        string BrokerExecutionId,
        string BrokerOrderId,
        string BrokerTradeId
    );
}
