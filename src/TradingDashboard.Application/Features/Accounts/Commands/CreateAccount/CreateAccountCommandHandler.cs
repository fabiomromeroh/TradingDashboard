using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Accounts.Dtos;
using TradingDashboard.Domain.Entities;
using TradingDashboard.Domain.Enums;

namespace TradingDashboard.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IUserRepository userRepository,
    IBrokerRepository brokerRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateAccountCommand, Result<AccountDto>>
{
    public async Task<Result<AccountDto>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null) return Result<AccountDto>.NotFound(nameof(User), command.UserId);

        var broker = await brokerRepository.GetByIdAsync(command.BrokerId, cancellationToken);
        if (broker is null) return Result<AccountDto>.NotFound(nameof(Broker), command.BrokerId);

        ImportSourceType importSourceType = Enum.Parse<ImportSourceType>(command.ImportSourceType);

        var account = Account.Create(command.Name, command.UserId, command.BrokerId, importSourceType);

        var accountId = await accountRepository.AddAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AccountDto>.Success(mapper.Map<AccountDto>(account));
    }
}
