using MediatR;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public class GetAllTradesQuery: IRequest<IEnumerable<TradeDto>>
{
}
