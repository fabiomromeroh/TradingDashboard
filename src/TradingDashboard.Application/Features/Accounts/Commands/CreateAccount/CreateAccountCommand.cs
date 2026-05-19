using MediatR;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand : IRequest<AccountDto>
{
    public string Name { get; init; } = string.Empty;
    public string Currency { get; init; } = "USD";
    public decimal InitialBalance { get; init; }
    public Guid UserId { get; init; }
    public Guid BrokerId { get; init; }
}
