using MediatR;

namespace TradingDashboard.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest;
