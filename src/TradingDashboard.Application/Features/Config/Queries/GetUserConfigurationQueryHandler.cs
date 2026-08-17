using MediatR;
using System.Text.Json;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Queries
{
    public class GetUserConfigurationQueryHandler : IRequestHandler<GetUserConfigurationQuery, Result<UserConfigurationDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetUserConfigurationQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<UserConfigurationDto>> Handle(GetUserConfigurationQuery request, CancellationToken cancellationToken)
        {
            var userConfig = await _userRepository.GetUserConfigurationAsync(request.UserId, cancellationToken);
            if (userConfig is null)
            {
                return Result<UserConfigurationDto>.NotFound("User configuration not found.");
            }
            QueryFilter filters = JsonSerializer.Deserialize<QueryFilter>(userConfig.FiltersJson);
            var userConfigDto = new UserConfigurationDto
            {
                Filters = filters
            };
            return Result<UserConfigurationDto>.Success(userConfigDto);
        }

    }
}
