using System.Text.Json.Serialization;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Config.Dtos
{
    /// <summary>
    /// Configuration DTO for user filters (date range, symbols, accounts, tags).
    /// Implements IUserConfig for polymorphic handling.
    /// </summary>
    public class FiltersConfig : IUserConfig
    {
        [JsonIgnore]
        public string ConfigType => "filters";

        [JsonPropertyName("filters")]
        [JsonPropertyOrder(1)]
        public ConfigFilter? Filters { get; set; }
    }
}

