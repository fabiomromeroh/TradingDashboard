namespace TradingDashboard.Application.Abstractions.Services.Import
{
    public interface IImportService
    {
        public Task<int> RebuildTradesAsync(Guid accountId, string[] symbols, CancellationToken ct);
    }
}
