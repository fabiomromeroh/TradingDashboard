using CsvHelper;
using System.Globalization;
using TradingDashboard.Application.Abstractions.Services.FileUpload.Models;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Abstractions.Services.Import.Models;
using TradingDashboard.Application.Extensions;

namespace TradingDashboard.Infrastructure.Services.Import.Ibkr
{
    public class IbkrCsvParser : IBrokerParser
    {
        public string BrokerName => "Interactive Brokers";

        // IBKR always exports in US Eastern Time — hardcoded, not user-provided
        private static readonly TimeZoneInfo IbkrTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // Windows
        // or "America/New_York" on Linux/Mac

        public ParsedImportResult Parse(byte[] fileBytes)
        {
            using var stream = new MemoryStream(fileBytes);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<IbkrRowMap>();

            var rows = new List<RawExecutionRow>();
            var errors = new List<string>();
            int rowNumber = 1;

            while (csv.Read())
            {
                try
                {
                    // CsvHelper reads into IbkrRawRecord using the map
                    var raw = csv.GetRecord<IbkrRawRecord>();

                    rows.Add(new RawExecutionRow(
                        RowNumber: rowNumber,
                        BrokerExecutionId: raw.ExecID,                          // unique per fill
                        BrokerOrderId: raw.OrderID,                         // groups partial fills
                        BrokerTradeId: raw.TradeID,                         // groups same intent
                        Symbol: raw.Symbol.Trim(),
                        Description: raw.Description.Trim(),
                        AssetClass: raw.AssetClass,                      // "STK", "ADR"
                        Currency: raw.CurrencyPrimary,
                        Side: raw.BuySell == "BUY" ? "Buy" : "Sell",
                        Quantity: Math.Abs(raw.Quantity),              // always positive
                        Price: raw.Price,
                        Commission: Math.Abs(raw.Commission),            // always positive
                        Exchange: raw.Exchange,
                        OrderType: raw.OrderType,                       // "LMT" / "MKT"
                        ExecutedAt: raw.DateTime.ParseDateTime(IbkrTimeZone)
                        ));

                }
                catch (Exception ex)
                {

                    errors.Add($"Row {rowNumber}: {ex.Message}");
                }

                rowNumber++;
            }

            return new ParsedImportResult(rows, errors);
        }
    }
}
