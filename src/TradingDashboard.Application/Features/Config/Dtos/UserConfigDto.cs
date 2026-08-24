using System.Text.Json.Serialization;

namespace TradingDashboard.Application.Features.Config.Dtos
{
    /// <summary>
    /// DTO for all user configurations, supporting polymorphic config types.
    /// Each configuration can be accessed by its ConfigType identifier.
    /// </summary>
    public class UserConfigDto
    {
        /// <summary>
        /// Collection of all user configuration items.
        /// Use IUserConfig interface to access base properties; cast to specific types as needed.
        /// </summary>
        [JsonPropertyName("configs")]
        public IEnumerable<IUserConfig> Configs { get; set; } = [];
    }
}
