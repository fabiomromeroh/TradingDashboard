using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;

namespace TradingDashboard.Application.Features.Config.Queries
{
    public record GetUserConfigurationQuery(Guid UserId) : IRequest<Result<UserConfigurationDto>>
    {
    }
}
