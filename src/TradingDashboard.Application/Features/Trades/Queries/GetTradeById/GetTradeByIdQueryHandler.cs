using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryHandler(ITradeQueryService tradeQueryService, IMapper mapper) : IRequestHandler<GetTradeByIdQuery, Result<TradeDto>>
{
    public async Task<Result<TradeDto>> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
    {
        var trade = await tradeQueryService.GetTradeAsync(request.Id, cancellationToken);
        if (trade is null) return Result<TradeDto>.NotFound(nameof(Trade), request.Id);

        return Result<TradeDto>.Success(mapper.Map<TradeDto>(trade));
    }
}
