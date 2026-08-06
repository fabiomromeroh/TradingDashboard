using MediatR;
using TradingDashboard.Application.Abstractions.Services.Dashboard;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;

namespace TradingDashboard.Application.Features.Dashboard.Queries
{
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryDto>>
    {
        private readonly IDashboardQueryService _dashboardQueryService;

        public GetDashboardSummaryQueryHandler(IDashboardQueryService dashboardQueryService)
        {
            _dashboardQueryService = dashboardQueryService;
        }
        public async Task<Result<DashboardSummaryDto>> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            //TODO - Get filter from db
            QueryFilter filter = new(request.AccountIds);

            var result = await _dashboardQueryService.GetDashboardSummaryAsync(filter, cancellationToken);

            return Result<DashboardSummaryDto>.Success(result);
        }
    }
}
