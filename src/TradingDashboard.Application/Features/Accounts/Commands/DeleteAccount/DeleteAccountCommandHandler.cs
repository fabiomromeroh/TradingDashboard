using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;

namespace TradingDashboard.Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), command.Id);

        account.Deactivate();
        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
