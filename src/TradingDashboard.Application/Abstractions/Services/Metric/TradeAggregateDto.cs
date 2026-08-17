namespace TradingDashboard.Application.Abstractions.Services.Metric
{
    public record TradeAggregateDto(
     int TotalTrades,
     int WinningTrades,
     int LosingTrades,
     decimal AvgWin,
     decimal AvgLoss,
     decimal GrossProfit,
     decimal GrossLoss,
     decimal NetPnl);
}
