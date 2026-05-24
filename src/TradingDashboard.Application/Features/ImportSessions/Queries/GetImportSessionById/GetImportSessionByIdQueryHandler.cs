using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;

public class GetImportSessionByIdQueryHandler : IRequestHandler<GetImportSessionByIdQuery, Result<ImportSessionDto>>
{
    private readonly IImportSessionRepository _importSessionRepository;
    private readonly IMapper _mapper;

    public GetImportSessionByIdQueryHandler(IImportSessionRepository importSessionRepository, IMapper mapper)
    {
        _importSessionRepository = importSessionRepository;
        _mapper = mapper;
    }

    public async Task<Result<ImportSessionDto>> Handle(GetImportSessionByIdQuery query, CancellationToken cancellationToken)
    {
        var session = await _importSessionRepository.GetByIdAsync(query.Id, cancellationToken);
        if (session is null) return Result<ImportSessionDto>.NotFound(nameof(Domain.Entities.ImportSession), query.Id);

        return Result<ImportSessionDto>.Success(_mapper.Map<ImportSessionDto>(session));
    }
}
