using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradesByAccountId;

public record GetTradesByAccountIdQuery(Guid UserId) : IRequest<Result<IEnumerable<TradeDto>>>;

