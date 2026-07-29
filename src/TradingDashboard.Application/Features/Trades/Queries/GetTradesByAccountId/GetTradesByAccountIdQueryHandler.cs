using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetAllTrades;

public class GetTradesByAccountIdQueryHandler : IRequestHandler<GetTradesByAccountIdQuery, Result<IEnumerable<TradeDto>>>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IMapper mapper;

    public GetTradesByAccountIdQueryHandler(ITradeRepository tradeRepository, IMapper mapper)
    {
        this.tradeRepository = tradeRepository;
        this.mapper = mapper;
    }

    public async Task<Result<IEnumerable<TradeDto>>> Handle(GetTradesByAccountIdQuery query, CancellationToken cancellationToken)
    {
        var trades = await tradeRepository.GetTradesByAccountId(query.accountIds, cancellationToken);
        return Result<IEnumerable<TradeDto>>.Success(mapper.Map<IEnumerable<TradeDto>>(trades));
    }
}
