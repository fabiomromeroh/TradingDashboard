using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradesPaginated;

public record GetTradesPaginatedQuery(Guid UserId, int PageSize, string? Cursor) : IRequest<Result<PaginatedResult<TradeDto>>>;
