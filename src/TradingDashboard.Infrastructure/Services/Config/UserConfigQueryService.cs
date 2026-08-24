using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Common.Configurations;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Infrastructure.Services.Config
{
    public class UserConfigQueryService(IUserRepository userRepository) : IUserConfigQueryService
    {
        public async Task<UserConfigDto> GetUserConfigAsync(Guid userId, CancellationToken cancellationToken)
        {
            var config = await userRepository.GetUserConfigurationAsync(userId, cancellationToken);
            var configs = new List<IUserConfig>();

            if (config is not null)
            {
                // Deserialize filters configuration if present
                if (!string.IsNullOrEmpty(config.FiltersJson) && config.FiltersJson != "{}")
                {
                    try
                    {
                        var queryFilter = JsonSerializer.Deserialize<ConfigFilter>(config.FiltersJson, AppJsonOptions.Default);
                        if (queryFilter != null)
                        {
                            configs.Add(new FiltersConfig { Filters = queryFilter });
                        }
                    }
                    catch
                    {
                        // Log deserialization error if needed, but continue
                    }
                }

                // Deserialize dashboard configuration if present
                if (!string.IsNullOrEmpty(config.WidgetLayoutJson) && config.WidgetLayoutJson != "[]")
                {
                    try
                    {
                        var dashboard = JsonSerializer.Deserialize<IEnumerable<ConfigDashboard>>(config.WidgetLayoutJson, AppJsonOptions.Default);
                        if (dashboard?.Any() == true)
                        {
                            configs.Add(new DashboardConfig { Widgets = dashboard });
                        }
                    }
                    catch
                    {
                        // Log deserialization error if needed, but continue
                    }
                }
            }
            else
            {
                // Return default filters if no config exists
                configs.Add(new FiltersConfig { Filters = new ConfigFilter([]) });
            }

            return new UserConfigDto { Configs = configs };
        }
    }
}
