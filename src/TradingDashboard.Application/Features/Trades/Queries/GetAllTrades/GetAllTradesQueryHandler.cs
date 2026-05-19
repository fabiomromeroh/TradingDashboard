using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public class GetAllTradesQueryHandler  : IRequestHandler<GetAllTradesQuery, IEnumerable<TradeDto>>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IMapper mapper;

    public GetAllTradesQueryHandler(ITradeRepository tradeRepository, IMapper mapper)
    {
        this.tradeRepository = tradeRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<TradeDto>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesAsync(cancellationToken);
        return mapper.Map<IEnumerable<TradeDto>>(trades); ;
    }
}
