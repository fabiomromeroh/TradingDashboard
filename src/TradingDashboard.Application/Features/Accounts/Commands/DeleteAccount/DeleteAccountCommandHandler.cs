using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : MediatR.IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(command.Id, cancellationToken);
        if (account is null) return Result.NotFound(nameof(Domain.Entities.Account), command.Id);

        account.Deactivate();
        await accountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
