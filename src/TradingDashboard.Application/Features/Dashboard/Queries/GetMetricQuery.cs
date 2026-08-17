using MediatR;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Dashboard.Queries
{
    public record GetMetricQuery(Guid UserId, string MetricType) : IRequest<Result<WidgetDto>>;


}
