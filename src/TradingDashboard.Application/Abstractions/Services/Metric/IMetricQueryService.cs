using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Dashboard.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services.Metric
{
    public interface IMetricQueryService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(QueryFilter filter, CancellationToken ct);

        Task<TradeAggregateDto> GetTradeAggregatesAsync(ISpecification<Trade> spec, CancellationToken ct);
        Task<IEnumerable<Trade>> GetTradesAsync(ISpecification<Trade> spec, CancellationToken ct);
    }
}
