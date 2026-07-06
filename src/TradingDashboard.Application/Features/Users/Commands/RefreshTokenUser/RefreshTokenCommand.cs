using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.RefreshTokenUser
{
    public record RefreshTokenCommand(string RawRefreshToken) : IRequest<Result<RefreshTokenDto>>
    {

    }
}
