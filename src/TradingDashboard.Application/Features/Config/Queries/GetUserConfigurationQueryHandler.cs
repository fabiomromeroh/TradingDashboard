using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Queries
{
    public class GetUserConfigurationQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserConfigurationQuery, Result<UserConfigurationDto>>
    {
        public async Task<Result<UserConfigurationDto>> Handle(GetUserConfigurationQuery request, CancellationToken cancellationToken)
        {
            var userConfig = await userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);
            if (userConfig is null)
            {
                return Result<UserConfigurationDto>.Success(new UserConfigurationDto
                {
                    Filters = new QueryFilter([])
                });
            }
            QueryFilter? filters = JsonSerializer.Deserialize<QueryFilter>(userConfig.FiltersJson);
            var userConfigDto = new UserConfigurationDto
            {
                Filters = filters
            };
            return Result<UserConfigurationDto>.Success(userConfigDto);
        }

    }
}
