using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Queries
{
    public class GetUserConfigurationQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserConfigurationQuery, Result<UserConfigDto>>
    {
        public async Task<Result<UserConfigDto>> Handle(GetUserConfigurationQuery request, CancellationToken cancellationToken)
        {
            var userConfig = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);
            if (userConfig is null)
            {
                return Result<UserConfigDto>.Success(new UserConfigDto
                {
                    Filters = new ConfigFilter([])
                });
            }
            ConfigFilter? filters = JsonSerializer.Deserialize<ConfigFilter>(userConfig.FiltersJson);
            var userConfigDto = new UserConfigDto
            {
                Filters = filters
            };
            return Result<UserConfigDto>.Success(userConfigDto);
        }

    }
}
