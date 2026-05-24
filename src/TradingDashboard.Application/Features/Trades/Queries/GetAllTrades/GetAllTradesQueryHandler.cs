using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public class GetAllTradesQueryHandler : IRequestHandler<GetAllTradesQuery, Result<IEnumerable<TradeDto>>>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IMapper mapper;

    public GetAllTradesQueryHandler(ITradeRepository tradeRepository, IMapper mapper)
    {
        this.tradeRepository = tradeRepository;
        this.mapper = mapper;
    }

    public async Task<Result<IEnumerable<TradeDto>>> Handle(GetAllTradesQuery request, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesAsync(cancellationToken);
        return Result<IEnumerable<TradeDto>>.Success(mapper.Map<IEnumerable<TradeDto>>(trades));
    }
}
