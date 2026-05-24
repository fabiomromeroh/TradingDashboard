using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

public record GetTradeByIdQuery : IRequest<Result<TradeDto>>
{
    public GetTradeByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
