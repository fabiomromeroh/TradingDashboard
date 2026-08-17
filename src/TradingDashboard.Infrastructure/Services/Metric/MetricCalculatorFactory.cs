using TradingDashboard.Application.Abstractions.Services.Metric;

namespace TradingDashboard.Infrastructure.Services.Metric
{
    public class MetricCalculatorFactory : IMetricCalculatorFactory
    {
        private readonly IDictionary<string, IMetricCalculator> _metricCalculators;

        public MetricCalculatorFactory(IEnumerable<IMetricCalculator> metricCalculators)
        {
            _metricCalculators = metricCalculators.ToDictionary(x => x.MetricType, StringComparer.OrdinalIgnoreCase);
        }
        public IMetricCalculator GetMetricCalculator(string metricName)
        {
            if (_metricCalculators.TryGetValue(metricName, out var calculator))
            {
                return calculator;
            }
            throw new KeyNotFoundException($"Metric calculator for '{metricName}' not found.");
        }
    }
}
