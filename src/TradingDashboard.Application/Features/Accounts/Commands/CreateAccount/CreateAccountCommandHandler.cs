using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBrokerRepository _brokerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccountCommandHandler(
        IAccountRepository accountRepository,
        IUserRepository userRepository,
        IBrokerRepository brokerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _brokerRepository = brokerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        _ = await _userRepository.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), command.UserId);

        _ = await _brokerRepository.GetByIdAsync(command.BrokerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Broker), command.BrokerId);

        var account = Account.Create(command.Name, command.Currency, command.InitialBalance, command.UserId, command.BrokerId);

        await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}
