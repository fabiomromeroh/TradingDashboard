using System.Globalization;

namespace TradingDashboard.API.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ParseDateTime(this string value)
        {

            return DateTime.ParseExact(value.Replace(";", " "), "yyyyMMdd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }
    }
}
