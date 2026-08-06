using MediatR;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<Result>;
