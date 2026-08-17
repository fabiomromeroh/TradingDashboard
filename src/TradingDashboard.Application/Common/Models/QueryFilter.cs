namespace TradingDashboard.Application.Common.Models
{
    public record QueryFilter(IReadOnlyCollection<Guid> AccountIds, DateOnly? DateFrom = null, DateOnly? DateTo = null);

}
