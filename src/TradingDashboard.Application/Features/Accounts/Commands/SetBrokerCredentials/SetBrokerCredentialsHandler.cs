using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Interfaces;
using TradingDashboard.Domain.Entities;

namespace TradingDashboard.Application.Features.Accounts.Commands.SetBrokerCredentials
{
    public class SetBrokerCredentialsHandler : IRequestHandler<SetBrokerCredentialsCommand, Result>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IBrokerAccountCredentialService brokerAccountCredentialService;
        private readonly IBrokerAccountCredentialRepository brokerAccountCredentialRepository;
        private readonly IUnitOfWork unitOfWork;

        public SetBrokerCredentialsHandler(IAccountRepository accountRepository,
            IBrokerAccountCredentialService brokerAccountCredentialService,
            IBrokerAccountCredentialRepository brokerAccountCredentialRepository,
            IUnitOfWork unitOfWork)
        {
            this.accountRepository = accountRepository;
            this.brokerAccountCredentialService = brokerAccountCredentialService;
            this.brokerAccountCredentialRepository = brokerAccountCredentialRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(SetBrokerCredentialsCommand command, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetByIdAsync(command.AccountId, cancellationToken);
            if (account is null) return Result.NotFound(nameof(Account), command.AccountId);

            var brokerCredentials = await brokerAccountCredentialRepository.GetAsync(command.AccountId, cancellationToken);

            if (brokerCredentials is null)
            {
                await brokerAccountCredentialService.CreateAsync(account.Id, account.Broker.Name, command.BrokerCredentials, cancellationToken);

            }
            else
            {
                await brokerAccountCredentialService.UpdateAsync(brokerCredentials, command.BrokerCredentials, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
