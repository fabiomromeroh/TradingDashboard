using MediatR;
using TradingDashboard.Application.Common;

namespace TradingDashboard.Application.Features.Users.Commands.LogoutUser
{
    public record LogoutCommand(string RefreshToken) : IRequest<Result>
    {
    }
}
