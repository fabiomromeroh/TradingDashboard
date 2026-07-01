using System.Globalization;

namespace TradingDashboard.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ParseDateTime(this string value, TimeZoneInfo timeZone)
        {
            var localDt = DateTime.ParseExact(value.Replace(";", " "), "yyyyMMdd HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);

            return TimeZoneInfo.ConvertTimeToUtc(localDt, timeZone); //Convert from local broker timezone to Utc
        }
    }
}
