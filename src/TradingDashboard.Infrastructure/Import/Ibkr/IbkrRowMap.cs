using CsvHelper.Configuration;

namespace TradingDashboard.Infrastructure.Import.Ibkr
{
    public class IbkrRowMap : ClassMap<IbkrRawRecord>
    {
        public IbkrRowMap()
        {
            Map(m => m.ClientAccountID).Name("ClientAccountID");
            Map(m => m.CurrencyPrimary).Name("CurrencyPrimary");
            Map(m => m.AssetClass).Name("AssetClass");
            Map(m => m.Symbol).Name("Symbol");
            Map(m => m.Description).Name("Description");
            Map(m => m.TradeID).Name("TradeID");
            Map(m => m.OrderID).Name("OrderID");
            Map(m => m.ExecID).Name("ExecID");
            Map(m => m.DateTime).Name("Date/Time");
            Map(m => m.Exchange).Name("Exchange");
            Map(m => m.BuySell).Name("Buy/Sell");
            Map(m => m.Quantity).Name("Quantity");
            Map(m => m.Price).Name("Price");
            Map(m => m.Commission).Name("Commission");
            Map(m => m.OrderType).Name("OrderType");
        }
    }
}
