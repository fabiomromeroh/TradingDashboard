namespace TradingDashboard.Application.Features.Trades.Dtos
{
    public record TradeDto(
            Guid Id,
            string Symbol,
            decimal EntryPrice,
            decimal? ClosePrice,
            decimal Quantity,
            decimal PositionSize,
            string Direction,
            string Status,
            DateTimeOffset OpenedAt,
            DateTimeOffset? ClosedAt,
            decimal TotalCommissions,
            decimal AverageEntryPrice,
            decimal? AverageClosePrice,
            decimal? NetReturn,
            decimal? PercentageReturn
            );
}
