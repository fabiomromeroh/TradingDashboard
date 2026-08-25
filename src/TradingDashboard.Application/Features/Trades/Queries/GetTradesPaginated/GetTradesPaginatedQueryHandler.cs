using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Services.Metric;
using TradingDashboard.Application.Abstractions.Services.Metric.Specifications;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Abstractions.Services.UserConfig;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Config.Dtos;
using TradingDashboard.Application.Features.Config.Extensions;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetTradesPaginated;

public class GetTradesPaginatedQueryHandler(ITradeQueryService tradeQuery, IMapper mapper, IUserConfigQueryService userConfigQuery) : IRequestHandler<GetTradesPaginatedQuery, Result<PaginatedResult<TradeDto>>>
{
    public async Task<Result<PaginatedResult<TradeDto>>> Handle(GetTradesPaginatedQuery query, CancellationToken cancellationToken)
    {
        UserConfigDto config = await userConfigQuery.GetUserConfigAsync(query.UserId, cancellationToken);

        ISpecification<Trade> spec = new MetricFilterSpecification(config.GetFilters());

        var paginatedTrades = await tradeQuery.GetTradesPaginatedAsync(query.UserId, spec, query.PageSize, query.Cursor, cancellationToken);

        var mappedTrades = mapper.Map<IEnumerable<TradeDto>>(paginatedTrades.Items);

        var result = new PaginatedResult<TradeDto>(mappedTrades, paginatedTrades.NextCursor, paginatedTrades.HasMore, paginatedTrades.TotalCount);

        return Result<PaginatedResult<TradeDto>>.Success(result);
    }
}
