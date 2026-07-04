namespace TradingDashboard.Application.Services.Import.Interfaces
{
    public interface IImportService
    {
        public Task RebuildTradesAsync(Guid accountId, string[] symbols, CancellationToken ct);
    }
}
