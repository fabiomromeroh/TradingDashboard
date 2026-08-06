using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;

namespace TradingDashboard.Application.Abstractions.Services.Dashboard
{
    public interface IDashboardQueryService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(QueryFilter filter, CancellationToken ct);

    }
}
