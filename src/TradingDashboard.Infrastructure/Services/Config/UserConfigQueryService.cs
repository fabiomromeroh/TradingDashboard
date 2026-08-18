using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Infrastructure.Services.Config
{
    public class UserConfigQueryService(IUserRepository userRepository) : IUserConfigQueryService
    {
        public async Task<UserConfigDto> GetUserConfigAsync(Guid userId, CancellationToken cancellationToken)
        {
            var config = await userRepository.GetUserConfigurationAsync(userId, cancellationToken);

            ConfigFilter? queryFilter = new([]);

            if (config is not null)

            {
                queryFilter = JsonSerializer.Deserialize<ConfigFilter>(config.FiltersJson) ?? new ConfigFilter([]);
            }


            return new UserConfigDto
            {
                Filters = queryFilter,
            };
        }
    }
}
