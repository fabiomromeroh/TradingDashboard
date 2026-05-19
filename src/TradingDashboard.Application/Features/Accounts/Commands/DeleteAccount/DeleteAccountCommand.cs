using MediatR;

namespace TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest;
