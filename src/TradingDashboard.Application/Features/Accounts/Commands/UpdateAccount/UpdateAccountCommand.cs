using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand : IRequest<Result<AccountDto>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Currency { get; init; } = "USD";
    public decimal InitialBalance { get; init; }
}
