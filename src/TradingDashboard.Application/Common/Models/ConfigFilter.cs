namespace TradingDashboard.Application.Common.Models
{
    public record ConfigFilter(IReadOnlyCollection<Guid> AccountIds, DateOnly? DateFrom = null, DateOnly? DateTo = null);

}
