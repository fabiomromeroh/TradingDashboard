using MediatR;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeHistory;

public class GetAllTradesQueryHandler  : IRequestHandler<GetAllTradesQuery, IEnumerable<TradeDto>>
{
    private readonly ITradeRepository tradeRepository;

    public GetAllTradesQueryHandler(ITradeRepository tradeRepository)
    {
        this.tradeRepository = tradeRepository;
    }

    public async Task<IEnumerable<TradeDto>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesAsync(cancellationToken);
        return trades.Select(t => new TradeDto(
            t.Id,
            t.Symbol,
            t.EntryPrice,
            t.Quantity,
            t.Direction.ToString(),
            t.Status.ToString(),
            t.OpenedAt
        ));
    }
}
