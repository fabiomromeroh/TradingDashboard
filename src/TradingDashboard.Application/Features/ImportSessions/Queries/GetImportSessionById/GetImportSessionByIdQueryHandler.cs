using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;

public class GetImportSessionByIdQueryHandler : IRequestHandler<GetImportSessionByIdQuery, ImportSessionDto>
{
    private readonly IImportSessionRepository _importSessionRepository;
    private readonly IMapper _mapper;

    public GetImportSessionByIdQueryHandler(IImportSessionRepository importSessionRepository, IMapper mapper)
    {
        _importSessionRepository = importSessionRepository;
        _mapper = mapper;
    }

    public async Task<ImportSessionDto> Handle(GetImportSessionByIdQuery query, CancellationToken cancellationToken)
    {
        var session = await _importSessionRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ImportSession), query.Id);

        return _mapper.Map<ImportSessionDto>(session);
    }
}
