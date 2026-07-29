using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBrokerRepository _brokerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBrokerAccountCredentialService _brokerAccountCredentialService;

    public CreateAccountCommandHandler(
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        IBrokerRepository brokerRepository,
        IUnitOfWork unitOfWork,
        IBrokerAccountCredentialService brokerAccountCredentialService,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _brokerRepository = brokerRepository;
        _unitOfWork = unitOfWork;
        _brokerAccountCredentialService = brokerAccountCredentialService;
        _mapper = mapper;
    }

    public async Task<Result<AccountDto>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null) return Result<AccountDto>.NotFound(nameof(User), command.UserId);

        var broker = await _brokerRepository.GetByIdAsync(command.BrokerId, cancellationToken);
        if (broker is null) return Result<AccountDto>.NotFound(nameof(Broker), command.BrokerId);

        ImportSourceType importSourceType = Enum.Parse<ImportSourceType>(command.ImportSourceType);

        var account = Account.Create(command.Name, command.UserId, command.BrokerId, importSourceType);

        var accountId = await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AccountDto>.Success(_mapper.Map<AccountDto>(account));
    }
}
