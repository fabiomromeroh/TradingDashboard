using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Models;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Services.Metric.Specifications;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Dashboard.Queries
{
    public class GetDashboardSummaryQueryHandler(IMetricCalculatorFactory factory, IUserRepository userRepository) : IRequestHandler<GetMetricQuery, Result<WidgetDto>>
    {
        public async Task<Result<WidgetDto>> Handle(GetMetricQuery request, CancellationToken cancellationToken)
        {
            var config = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            QueryFilter? queryFilter = new([]);

            if (config is not null)
            {

                queryFilter = JsonSerializer.Deserialize<QueryFilter>(config.FiltersJson) ?? new QueryFilter([]);

            }

            ISpecification<Trade> spec = new MetricFilterSpecification(queryFilter);

            var metricCalculator = factory.GetMetricCalculator(request.MetricType);

            var payload = await metricCalculator.CalculateMetricAsync(spec, cancellationToken);

            WidgetDto result = WidgetDtoMapper.Wrap(metricCalculator.RenderType, metricCalculator.MetricType, payload);

            return Result<WidgetDto>.Success(result);
        }
    }
}
