using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Specifications;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradesByAccountId;

public class GetTradesByAccountIdQueryHandler(ITradeQueryService tradeQuery, IMapper mapper, IUserConfigQueryService userConfigQuery) : IRequestHandler<GetTradesByAccountIdQuery, Result<IEnumerable<TradeDto>>>
{
    public async Task<Result<IEnumerable<TradeDto>>> Handle(GetTradesByAccountIdQuery query, CancellationToken cancellationToken)
    {

        UserConfigDto config = await userConfigQuery.GetUserConfigAsync(query.UserId, cancellationToken);

        ISpecification<Trade> spec = new MetricFilterSpecification(config.Filters);

        var trades = await tradeQuery.GetTradesAsync(query.UserId, spec, cancellationToken);

        return Result<IEnumerable<TradeDto>>.Success(mapper.Map<IEnumerable<TradeDto>>(trades));
    }
}
