using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;

public class GetImportSessionsByAccountQueryHandler
    : IRequestHandler<GetImportSessionsByAccountQuery, IEnumerable<ImportSessionDto>>
{
    private readonly IImportSessionRepository _importSessionRepository;
    private readonly IMapper _mapper;

    public GetImportSessionsByAccountQueryHandler(IImportSessionRepository importSessionRepository, IMapper mapper)
    {
        _importSessionRepository = importSessionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ImportSessionDto>> Handle(
        GetImportSessionsByAccountQuery query, CancellationToken cancellationToken)
    {
        var sessions = await _importSessionRepository.GetAllByAccountIdAsync(query.AccountId, cancellationToken);
        return _mapper.Map<IEnumerable<ImportSessionDto>>(sessions);
    }
}
