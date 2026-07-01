namespace TradingDashboard.Application.Common.Exceptions
{
    public static class StringExtensions
    {
        public static Domain.Enums.Side ToEnum(this string value)
        {
            return value == "Buy" ? Domain.Enums.Side.Buy : Domain.Enums.Side.Sell;
        }
    }
}
