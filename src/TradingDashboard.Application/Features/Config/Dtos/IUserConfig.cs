using System.Text.Json.Serialization;

namespace TradingDashboard.Application.Features.Config.Dtos
{
    /// <summary>
    /// Base interface for all user configuration types.
    /// Allows polymorphic handling of different configuration DTOs in a single command/query.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "configType")]
    [JsonDerivedType(typeof(FiltersConfig), typeDiscriminator: "filters")]
    [JsonDerivedType(typeof(DashboardConfig), typeDiscriminator: "dashboard")]
    public interface IUserConfig
    {
        /// <summary>
        /// Gets the type identifier for this configuration.
        /// Used for discriminating between different config types during serialization/deserialization.
        /// </summary>
        string ConfigType { get; }
    }
}

