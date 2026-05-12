using MediatR;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

public record GetTradeByIdQuery: IRequest<TradeDto>
{
    public GetTradeByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
