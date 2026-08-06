using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;

namespace TradingDashboard.Application.Features.Dashboard.Queries
{
    public record GetDashboardSummaryQuery(IReadOnlyCollection<Guid> AccountIds) : IRequest<Result<DashboardSummaryDto>>;


}
