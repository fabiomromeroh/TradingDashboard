namespace TradingDashboard.Application.Abstractions.Services.Metric
{
    public interface IMetricCalculatorFactory
    {
        IMetricCalculator GetMetricCalculator(string metricType);
    }
}
