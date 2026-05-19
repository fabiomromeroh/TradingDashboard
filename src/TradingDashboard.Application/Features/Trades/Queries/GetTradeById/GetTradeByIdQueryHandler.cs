using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Trades.Dtos;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradeById;

public class GetTradeByIdQueryHandler: IRequestHandler<GetTradeByIdQuery, TradeDto>
{
    private readonly ITradeRepository tradeRepository;
    private readonly IMapper mapper;

    public GetTradeByIdQueryHandler(ITradeRepository tradeRepository, IMapper mapper)
    {
        this.tradeRepository = tradeRepository;
        this.mapper = mapper;
    }
    public async Task<TradeDto> Handle(GetTradeByIdQuery request, CancellationToken cancellationToken)
    {
        var trade = await tradeRepository.GetTradeAsync(request.Id, cancellationToken);

        if (trade == null)
        {
            throw new NotFoundException(nameof(trade), request.Id);
        }

        var tradeDto = mapper.Map<TradeDto>(trade);

        return tradeDto;
    }

 
}
