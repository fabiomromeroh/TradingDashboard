using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryHandler : IRequestHandler<GetTradeByIdQuery, Result<TradeDto>>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IMapper mapper;

    public GetTradeByIdQueryHandler(ITradeRepository tradeRepository, IMapper mapper)
    {
        this.tradeRepository = tradeRepository;
        this.mapper = mapper;
    }

    public async Task<Result<TradeDto>> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
    {
        var trade = await tradeRepository.GetTradeAsync(request.Id, cancellationToken);
        if (trade is null) return Result<TradeDto>.NotFound(nameof(Trade), request.Id);

        return Result<TradeDto>.Success(mapper.Map<TradeDto>(trade));
    }
}
