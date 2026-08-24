using System.Text.Json.Serialization;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Config.Dtos
{
    /// <summary>
    /// Configuration DTO for dashboard widget layout and visibility.
    /// Implements IUserConfig for polymorphic handling.
    /// </summary>
    public class DashboardConfig : IUserConfig
    {
        [JsonIgnore]
        public string ConfigType => "dashboard";

        [JsonPropertyName("widgets")]
        [JsonPropertyOrder(1)]
        public IEnumerable<ConfigDashboard>? Widgets { get; set; } = [];
    }
}
