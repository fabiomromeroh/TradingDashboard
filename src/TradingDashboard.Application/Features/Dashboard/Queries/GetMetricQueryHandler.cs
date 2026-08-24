using MediatR;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Abstractions.Services.Metric.Specifications;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Extensions;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Dashboard.Queries
{
    public class GetDashboardSummaryQueryHandler(IMetricCalculatorFactory factory, IUserConfigQueryService userConfigQueryService) : IRequestHandler<GetMetricQuery, Result<WidgetDto>>
    {
        public async Task<Result<WidgetDto>> Handle(GetMetricQuery request, CancellationToken cancellationToken)
        {
            var config = await userConfigQueryService.GetUserConfigAsync(request.UserId, cancellationToken);

            ISpecification<Trade> spec = new MetricFilterSpecification(config.GetFilters());

            var metricCalculator = factory.GetMetricCalculator(request.MetricType);

            var payload = await metricCalculator.CalculateMetricAsync(request.UserId, spec, cancellationToken);

            WidgetDto result = WidgetDtoMapper.Wrap(metricCalculator.RenderType, metricCalculator.MetricType, payload);

            return Result<WidgetDto>.Success(result);
        }
    }
}
