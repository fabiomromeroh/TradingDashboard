using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public class GetAllTradesQuery : IRequest<Result<IEnumerable<TradeDto>>>
{
}
