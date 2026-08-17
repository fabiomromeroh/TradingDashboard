using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Abstractions.Services.Metric;

public interface IMetricCalculator
{
    string MetricType { get; }        // e.g. "win-rate" — maps to widgetType
    string RenderType { get; }        // e.g. "gauge" — tells the wrapper which payload shape to expect
    Task<object> CalculateMetricAsync(ISpecification<Trade> spec, CancellationToken cancellationToken);
}

