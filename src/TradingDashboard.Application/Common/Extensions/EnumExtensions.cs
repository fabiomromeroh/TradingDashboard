using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static TradeDirection ToTradeDirection(this Side side)
        {
            return side == Side.Buy ? TradeDirection.Long : TradeDirection.Short;
        }
    }
}
