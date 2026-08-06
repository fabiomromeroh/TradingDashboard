namespace TradingDashboard.Application.Common.Models
{
    public record QueryFilter(IReadOnlyCollection<Guid> AccountIds, DateOnly? StartDate = null, DateOnly? EndDate = null);

}
