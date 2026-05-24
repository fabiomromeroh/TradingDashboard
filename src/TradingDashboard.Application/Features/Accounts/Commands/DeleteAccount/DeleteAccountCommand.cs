using MediatR;
using TradingDashboard.Application.Common;

namespace TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<Result>;
