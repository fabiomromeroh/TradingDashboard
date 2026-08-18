using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateAccountCommand, Result<AccountDto>>
{
    public async Task<Result<AccountDto>> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(command.Id, cancellationToken);
        if (account is null) return Result<AccountDto>.NotFound(nameof(Domain.Entities.Account), command.Id);

        account.Update(command.Name, command.Currency, command.InitialBalance);

        await accountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AccountDto>.Success(mapper.Map<AccountDto>(account));
    }
}
