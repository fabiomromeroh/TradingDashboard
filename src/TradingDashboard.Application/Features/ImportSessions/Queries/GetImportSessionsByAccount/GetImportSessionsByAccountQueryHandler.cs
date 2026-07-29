using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;

public class GetImportSessionsByAccountQueryHandler
    : IRequestHandler<GetImportSessionsByAccountQuery, Result<IEnumerable<ImportSessionDto>>>
{
    private readonly IImportSessionRepository _importSessionRepository;
    private readonly IMapper _mapper;

    public GetImportSessionsByAccountQueryHandler(IImportSessionRepository importSessionRepository, IMapper mapper)
    {
        _importSessionRepository = importSessionRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ImportSessionDto>>> Handle(
        GetImportSessionsByAccountQuery query, CancellationToken cancellationToken)
    {
        var sessions = await _importSessionRepository.GetAllByAccountIdAsync(query.AccountId, cancellationToken);
        return Result<IEnumerable<ImportSessionDto>>.Success(_mapper.Map<IEnumerable<ImportSessionDto>>(sessions));
    }
}
