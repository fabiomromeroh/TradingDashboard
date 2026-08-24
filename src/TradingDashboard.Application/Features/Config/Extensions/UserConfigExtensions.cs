using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Extensions
{
    /// <summary>
    /// Extension methods for working with polymorphic user configurations.
    /// </summary>
    public static class UserConfigExtensions
    {
        /// <summary>
        /// Extracts the ConfigFilter from the collection of user configurations.
        /// </summary>
        public static ConfigFilter? GetFilters(this UserConfigDto config)
        {
            return config.Configs
                .OfType<FiltersConfig>()
                .FirstOrDefault()
                ?.Filters;
        }

        /// <summary>
        /// Extracts the widget collection from the dashboard configuration.
        /// </summary>
        public static IEnumerable<ConfigDashboard>? GetWidgets(this UserConfigDto config)
        {
            return config.Configs
                .OfType<DashboardConfig>()
                .FirstOrDefault()
                ?.Widgets;
        }

        /// <summary>
        /// Gets a specific configuration by type.
        /// </summary>
        public static T? GetConfig<T>(this UserConfigDto config) where T : class, IUserConfig
        {
            return config.Configs
                .OfType<T>()
                .FirstOrDefault();
        }
    }
}
