using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.LoginUser;

public record LoginUserCommand : IRequest<Result<LoginResponseDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
