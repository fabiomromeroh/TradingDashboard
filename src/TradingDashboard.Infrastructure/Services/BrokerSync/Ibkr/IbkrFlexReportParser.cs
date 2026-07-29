using System.Globalization;
using System.Xml.Linq;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.BrokerSync.Ibkr;

namespace TradingDashboard.Infrastructure.Services.BrokerSync.Ibkr
{
    public class IbkrFlexReportParser : IIbkrFlexReportParser
    {
        public IReadOnlyList<ParsedExecution> Parse(XDocument report)
        {
            var tradeElements = report.Descendants("TradeConfirm").ToList();

            if (tradeElements.Count == 0)
                return [];

            var executions = new List<ParsedExecution>(tradeElements.Count);

            var rowNumber = 1;
            foreach (var trade in tradeElements)
            {
                executions.Add(MapTrade(trade, rowNumber));
                rowNumber++;
            }

            return executions;
        }

        private static ParsedExecution MapTrade(XElement trade, int rowNumber)
        {
            return new ParsedExecution
            {
                RowNumber = rowNumber,
                BrokerExecutionId = GetAttr(trade, "execID") ?? throw MissingField("execID"),
                BrokerOrderId = GetAttr(trade, "orderID") ?? throw MissingField("orderID"),
                BrokerTradeId = GetAttr(trade, "tradeID") ?? throw MissingField("tradeID"),
                TransactionId = GetAttr(trade, "extExecID"), // no true transactionID on TradeConfirm

                Symbol = GetAttr(trade, "symbol") ?? throw MissingField("symbol"),
                UnderlyingSymbol = GetAttr(trade, "underlyingSymbol"),
                Description = GetAttr(trade, "description") ?? string.Empty,
                AssetClass = GetAttr(trade, "assetCategory") ?? throw MissingField("assetCategory"),
                Currency = GetAttr(trade, "currency") ?? throw MissingField("currency"),

                Side = MapSide(GetAttr(trade, "buySell")),
                Quantity = Math.Abs(ParseDecimal(GetAttr(trade, "quantity"))),
                Price = ParseDecimal(GetAttr(trade, "price")),
                Commission = Math.Abs(ParseDecimal(GetAttr(trade, "commission"))),
                CommissionCurrency = GetAttr(trade, "commissionCurrency"),
                Proceeds = ParseNullableDecimal(GetAttr(trade, "proceeds")),
                NetCash = ParseNullableDecimal(GetAttr(trade, "netCash")),
                Exchange = GetAttr(trade, "exchange"),
                OrderType = GetAttr(trade, "orderType"),

                OpenCloseIndicator = null, // not available on TradeConfirm; use "code" if needed later
                RealizedPnl = null,        // not available on TradeConfirm

                Strike = ParseNullableDecimal(GetAttr(trade, "strike")),
                Expiry = ParseNullableDate(GetAttr(trade, "expiry")),
                PutCall = GetAttr(trade, "putCall"),
                Multiplier = ParseNullableDecimal(GetAttr(trade, "multiplier")),

                ExecutedAt = ParseDateTimeOffset(GetAttr(trade, "dateTime")),

                BrokerName = "Interactive Brokers",
                SourceType = "ApiSync"
            };
        }

        private static string? GetAttr(XElement element, string name) =>
            element.Attribute(name)?.Value;

        private static string MapSide(string? value) => value switch
        {
            "BUY" => "Buy",
            "SELL" => "Sell",
            _ => throw new IbkrFlexParseException($"Unknown Buy/Sell value: '{value}'")
        };

        private static decimal ParseDecimal(string? value) =>
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : throw new IbkrFlexParseException($"Invalid decimal value: '{value}'");

        private static decimal? ParseNullableDecimal(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : ParseDecimal(value);

        private static DateOnly ParseDate(string? value) =>
            DateOnly.ParseExact(value!, "yyyyMMdd", CultureInfo.InvariantCulture);

        private static DateOnly? ParseNullableDate(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : ParseDate(value);

        private static DateTimeOffset ParseDateTimeOffset(string? value)
        {
            var parts = value!.Split(';');
            var datePart = DateOnly.ParseExact(parts[0], "yyyyMMdd", CultureInfo.InvariantCulture);
            var timePart = parts.Length > 1
                ? TimeOnly.ParseExact(parts[1], "HHmmss", CultureInfo.InvariantCulture)
                : TimeOnly.MinValue;

            // IBKR Flex timestamps are Eastern Time, same convention as your CSV parser
            var eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var local = datePart.ToDateTime(timePart);
            var offset = eastern.GetUtcOffset(local);
            return new DateTimeOffset(local, offset);
        }

        private static IbkrFlexParseException MissingField(string fieldName) =>
            new($"Missing required Flex XML attribute: '{fieldName}'");
    }


}
