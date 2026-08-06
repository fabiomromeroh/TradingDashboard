using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Application.Interfaces;

namespace TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, Result<AccountDto>>
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

    public async Task<Result<AccountDto>> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(command.Id, cancellationToken);
        if (account is null) return Result<AccountDto>.NotFound(nameof(Domain.Entities.Account), command.Id);

        account.Update(command.Name, command.Currency, command.InitialBalance);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AccountDto>.Success(_mapper.Map<AccountDto>(account));
    }
}
