using MediatR;
using TradingDashboard.Application.Common;

namespace TradingDashboard.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<Result>;
