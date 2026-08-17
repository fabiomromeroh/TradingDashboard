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
    public class GetDashboardSummaryQueryHandler : IRequestHandler<GetMetricQuery, Result<WidgetDto>>
    {
        private readonly IMetricCalculatorFactory _factory;
        private readonly IUserRepository _userRepository;

        public GetDashboardSummaryQueryHandler(IMetricCalculatorFactory factory, IUserRepository userRepository)
        {
            _factory = factory;
            _userRepository = userRepository;
        }
        public async Task<Result<WidgetDto>> Handle(GetMetricQuery request, CancellationToken cancellationToken)
        {
            //TODO - Get filter from db
            var config = await _userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            var queryFilter = JsonSerializer.Deserialize<QueryFilter>(config.FiltersJson);

            ISpecification<Trade> spec = new MetricFilterSpecification(queryFilter);

            var metricCalculator = _factory.GetMetricCalculator(request.MetricType);

            var payload = await metricCalculator.CalculateMetricAsync(spec, cancellationToken);

            WidgetDto result = WidgetDtoMapper.Wrap(metricCalculator.RenderType, metricCalculator.MetricType, payload);

            return Result<WidgetDto>.Success(result);
        }
    }
}
