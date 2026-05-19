using MediatR;
using TradingDashboard.Application.Features.Users.Dtos;

namespace TradingDashboard.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<UserDto>
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
