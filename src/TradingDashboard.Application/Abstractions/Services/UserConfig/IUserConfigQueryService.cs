using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Abstractions.Services.UserConfig
{
    public interface IUserConfigQueryService
    {
        Task<UserConfigDto> GetUserConfigAsync(Guid userId, CancellationToken cancellationToken);
    }
}
