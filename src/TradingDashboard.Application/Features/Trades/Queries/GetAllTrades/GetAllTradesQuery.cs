using MediatR;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeHistory;

public class GetAllTradesQuery: IRequest<IEnumerable<TradeDto>>
{
}
