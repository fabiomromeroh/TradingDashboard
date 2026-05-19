using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.ImportSessions.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.CreateImportSession;

public class CreateImportSessionCommandHandler : IRequestHandler<CreateImportSessionCommand, ImportSessionDto>
{
    private readonly IImportSessionRepository _importSessionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateImportSessionCommandHandler(
        IImportSessionRepository importSessionRepository,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _importSessionRepository = importSessionRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ImportSessionDto> Handle(CreateImportSessionCommand command, CancellationToken cancellationToken)
    {
        _ = await _accountRepository.GetByIdAsync(command.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Account), command.AccountId);

        var session = ImportSession.Create(command.FileName, command.AccountId);

        await _importSessionRepository.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ImportSessionDto>(session);
    }
}
