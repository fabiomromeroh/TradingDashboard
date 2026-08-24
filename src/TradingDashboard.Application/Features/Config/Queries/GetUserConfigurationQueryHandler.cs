using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Configurations;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Queries
{
    public class GetUserConfigurationQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserConfigurationQuery, Result<UserConfigDto>>
    {
        public async Task<Result<UserConfigDto>> Handle(GetUserConfigurationQuery request, CancellationToken cancellationToken)
        {
            var userConfig = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);

            var configs = new List<IUserConfig>();

            if (userConfig is null)
            {
                return Result<UserConfigDto>.Success(new UserConfigDto { Configs = configs });
            }

            // Deserialize filters configuration if present
            if (!string.IsNullOrEmpty(userConfig.FiltersJson) && userConfig.FiltersJson != "{}")
            {
                try
                {
                    var filters = JsonSerializer.Deserialize<ConfigFilter>(userConfig.FiltersJson, AppJsonOptions.Default);
                    if (filters != null)
                    {
                        configs.Add(new FiltersConfig { Filters = filters });
                    }
                }
                catch
                {
                    // Log deserialization error if needed, but continue
                }
            }

            // Deserialize dashboard configuration if present
            if (!string.IsNullOrEmpty(userConfig.WidgetLayoutJson) && userConfig.WidgetLayoutJson != "[]")
            {
                try
                {
                    var dashboard = JsonSerializer.Deserialize<IEnumerable<ConfigDashboard>>(userConfig.WidgetLayoutJson, AppJsonOptions.Default);
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

            var userConfigDto = new UserConfigDto { Configs = configs };
            return Result<UserConfigDto>.Success(userConfigDto);
        }
    }
}

