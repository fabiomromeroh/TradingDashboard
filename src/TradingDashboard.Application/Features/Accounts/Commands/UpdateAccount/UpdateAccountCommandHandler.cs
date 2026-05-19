using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), command.Id);

        account.Update(command.Name, command.Currency, command.InitialBalance);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AccountDto>(account);
    }
}
