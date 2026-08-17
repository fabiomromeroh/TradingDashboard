namespace TradingDashboard.Application.Abstractions.Services.Metric.Models
{
    public enum MetricTone { Default, Success, Danger, Warning, Muted }

    public record StatDto(string Label, string Value, MetricTone? Tone = null);

    public record MetricPayloadDto(string DisplayValue, string Description, MetricTone? Tone = null, List<AreaChartPointDto>? Points = null);
    public record GaugePayloadDto(string DisplayValue, string Description, decimal Percent, List<StatDto> Stats);
    public record RingPayloadDto(string DisplayValue, string Description, decimal Numerator, decimal Denominator, List<StatDto>? Stats = null);
    public record RangePayloadDto(string DisplayValue, string Description, StatDto Left, StatDto Right, decimal Ratio);
    public record AreaChartPayloadDto(string Description, List<AreaChartPointDto> Points);
    public record AreaChartPointDto(string X, decimal Y);
    public record BarChartPayloadDto(string Description, List<AreaChartPointDto> Points, bool? ColorByValue = null);
    public record DistributionPayloadDto(string Description, List<DistributionSegmentDto> Segments);
    public record DistributionSegmentDto(string Name, decimal Value);
}
