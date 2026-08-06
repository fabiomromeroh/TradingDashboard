using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Trades.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Trades.Queries.GetExecutionsByTradeId
{
    public class GetExecutionsByTradeIdHandler : IRequestHandler<GetExecutionsByTradeIdQuery, Result<IEnumerable<ExecutionDto>>>
    {
        private readonly IExecutionRepository executionRepository;
        private readonly ITradeRepository tradeRepository;
        private readonly IMapper mapper;

        public GetExecutionsByTradeIdHandler(IExecutionRepository executionRepository, ITradeRepository tradeRepository, IMapper mapper)
        {
            this.executionRepository = executionRepository;
            this.tradeRepository = tradeRepository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<ExecutionDto>>> Handle(GetExecutionsByTradeIdQuery query, CancellationToken ct)
        {
            var exists = await tradeRepository.GetTradeAsync(query.tradeId, ct);

            if (exists is null) return Result<IEnumerable<ExecutionDto>>.NotFound(nameof(Trade), query.tradeId);

            var executions = await executionRepository.GetAllByTradeIdAsync(query.tradeId, ct);

            return Result<IEnumerable<ExecutionDto>>.Success(mapper.Map<IEnumerable<ExecutionDto>>(executions));
        }
    }
}
