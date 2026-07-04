using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetExecutionsByTradeId
{
    public record GetExecutionsByTradeIdQuery(Guid tradeId) : IRequest<Result<IEnumerable<ExecutionDto>>>
    {

    }
}
