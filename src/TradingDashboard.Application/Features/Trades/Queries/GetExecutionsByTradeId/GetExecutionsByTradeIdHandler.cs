using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetExecutionsByTradeId
{
    public class GetExecutionsByTradeIdHandler(IExecutionRepository executionRepository, ITradeQueryService tradeQueryService, IMapper mapper) : IRequestHandler<GetExecutionsByTradeIdQuery, Result<IEnumerable<ExecutionDto>>>
    {
        public async Task<Result<IEnumerable<ExecutionDto>>> Handle(GetExecutionsByTradeIdQuery query, CancellationToken ct)
        {
            var exists = await tradeQueryService.GetTradeAsync(query.tradeId, ct);

            if (exists is null) return Result<IEnumerable<ExecutionDto>>.NotFound(nameof(Trade), query.tradeId);

            var executions = await executionRepository.GetAllByTradeIdAsync(query.tradeId, ct);

            return Result<IEnumerable<ExecutionDto>>.Success(mapper.Map<IEnumerable<ExecutionDto>>(executions));
        }
    }
}
