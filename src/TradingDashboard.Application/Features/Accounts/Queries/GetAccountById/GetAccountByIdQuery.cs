using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<Result<AccountDto>>;
