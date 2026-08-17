using System.Text.Json.Serialization;

namespace TradingDashboard.Application.Abstractions.Services.Metric.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "renderType")]
    [JsonDerivedType(typeof(MetricWidgetDto), typeDiscriminator: "metric")]
    [JsonDerivedType(typeof(GaugeWidgetDto), typeDiscriminator: "gauge")]
    [JsonDerivedType(typeof(RingWidgetDto), typeDiscriminator: "ring")]
    [JsonDerivedType(typeof(RangeWidgetDto), typeDiscriminator: "range")]
    [JsonDerivedType(typeof(AreaChartWidgetDto), typeDiscriminator: "area-chart")]
    [JsonDerivedType(typeof(BarChartWidgetDto), typeDiscriminator: "bar-chart")]
    [JsonDerivedType(typeof(DistributionWidgetDto), typeDiscriminator: "distribution")]
    public abstract record WidgetDto(string WidgetType, string RenderType);

    public record MetricWidgetDto(string WidgetType, string RenderType, MetricPayloadDto Payload) : WidgetDto(WidgetType, RenderType);
    public record GaugeWidgetDto(string WidgetType, string RenderType, GaugePayloadDto Payload) : WidgetDto(WidgetType, RenderType);
    public record RingWidgetDto(string WidgetType, string RenderType, RingPayloadDto Payload) : WidgetDto(WidgetType, RenderType);
    public record RangeWidgetDto(string WidgetType, string RenderType, RangePayloadDto Payload) : WidgetDto(WidgetType, RenderType);
    public record AreaChartWidgetDto(string WidgetType, string RenderType, AreaChartPayloadDto Payload) : WidgetDto(WidgetType, RenderType);
    public record BarChartWidgetDto(string WidgetType, string RenderType, BarChartPayloadDto Payload) : WidgetDto(WidgetType, RenderType);

    public record DistributionWidgetDto(string WidgetType, string RenderType, DistributionPayloadDto Payload) : WidgetDto(WidgetType, RenderType);
}
