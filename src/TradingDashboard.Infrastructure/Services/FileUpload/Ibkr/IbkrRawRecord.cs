namespace TradingDashboard.Infrastructure.Services.Import.Ibkr
{
    public class IbkrRawRecord
    {
        public string ClientAccountID { get; set; } = "";
        public string CurrencyPrimary { get; set; } = "";
        public string AssetClass { get; set; } = "";
        public string Symbol { get; set; } = "";
        public string Description { get; set; } = "";
        public string TradeID { get; set; } = "";
        public string OrderID { get; set; } = "";
        public string ExecID { get; set; } = "";   // ← unique key per execution
        public string DateTime { get; set; } = "";   // "20260331;095834"
        public string Exchange { get; set; } = "";
        public string BuySell { get; set; } = "";   // "BUY" or "SELL"
        public decimal Quantity { get; set; }         // negative on sells
        public decimal Price { get; set; }
        public decimal Commission { get; set; }         // total, already negative
        public string OrderType { get; set; } = "";   // "LMT" or "MKT"
    }
}
