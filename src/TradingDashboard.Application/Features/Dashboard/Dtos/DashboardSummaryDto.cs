namespace TradingDashboard.Application.Features.Dashboard.Dtos
{
    public class DashboardSummaryDto
    {
        public decimal NetPnl { get; set; } = decimal.Zero;
        public int TradeCount { get; set; } = 0;
        public int WinCount { get; set; } = 0;
        public int LossCount { get; set; } = 0;
        public decimal AverageWin { get; set; } = decimal.Zero;
        public decimal AverageLoss { get; set; } = decimal.Zero;
    }
}
