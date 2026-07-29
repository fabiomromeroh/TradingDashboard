using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(string Name, string ImportSourceType, Guid BrokerId, Guid UserId) : IRequest<Result<AccountDto>>;

