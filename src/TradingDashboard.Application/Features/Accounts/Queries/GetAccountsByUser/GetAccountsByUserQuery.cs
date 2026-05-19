using MediatR;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

public record GetAccountsByUserQuery(Guid UserId) : IRequest<IEnumerable<AccountDto>>;
