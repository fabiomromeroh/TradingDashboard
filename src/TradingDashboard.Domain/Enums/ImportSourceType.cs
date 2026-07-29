namespace TradingDashboard.Domain.Enums
{
    public enum ImportSourceType
    {
        BrokerSync,     // scheduled or manual API sync
        FileUpload,     // user uploads CSV/PDF/XLSX
        ManualEntry     // future: user types a trade manually
    }
}
