using MediatR;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand : IRequest<UserDto>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
