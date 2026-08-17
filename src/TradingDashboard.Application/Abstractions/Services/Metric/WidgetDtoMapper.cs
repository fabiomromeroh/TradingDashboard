using TradingDashboard.Application.Abstractions.Services.Metric.Models;

namespace TradingDashboard.Application.Abstractions.Services.Metric
{
    public static class WidgetDtoMapper
    {
        public static WidgetDto Wrap(string renderType, string widgetType, object payload) => renderType switch
        {
            "metric" => new MetricWidgetDto(widgetType, renderType, (MetricPayloadDto)payload),
            "gauge" => new GaugeWidgetDto(widgetType, renderType, (GaugePayloadDto)payload),
            "ring" => new RingWidgetDto(widgetType, renderType, (RingPayloadDto)payload),
            "range" => new RangeWidgetDto(widgetType, renderType, (RangePayloadDto)payload),
            "area-chart" => new AreaChartWidgetDto(widgetType, renderType, (AreaChartPayloadDto)payload),
            "bar-chart" => new BarChartWidgetDto(widgetType, renderType, (BarChartPayloadDto)payload),
            "distribution" => new DistributionWidgetDto(widgetType, renderType, (DistributionPayloadDto)payload),
            _ => throw new NotSupportedException($"Unknown render type: {renderType}")
        };
    }
}
