using System.Text.Json;
using System.Text.Json.Serialization;

namespace TradingDashboard.Application.Common.Configurations
{
    public static class AppJsonOptions
    {
        public static void Configure(JsonSerializerOptions options)
        {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        public static readonly JsonSerializerOptions Default = CreateDefault();

        private static JsonSerializerOptions CreateDefault()
        {
            var options = new JsonSerializerOptions();
            Configure(options);
            return options;
        }
    }
}
