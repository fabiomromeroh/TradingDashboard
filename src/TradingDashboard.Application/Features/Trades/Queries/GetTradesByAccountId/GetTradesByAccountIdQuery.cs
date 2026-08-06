using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public record GetTradesByAccountIdQuery(List<Guid> accountIds) : IRequest<Result<IEnumerable<TradeDto>>>;

