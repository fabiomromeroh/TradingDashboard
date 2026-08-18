using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Abstractions.Services.Trades;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQueryHandler(IAccountRepository accountRepository,
    IBrokerAccountCredentialService brokerAccountCredentialService,
    ITradeQueryService tradeQueryService,
    IMapper mapper) : IRequestHandler<GetAccountsByUserQuery, Result<IEnumerable<AccountDto>>>
{
    public async Task<Result<IEnumerable<AccountDto>>> Handle(GetAccountsByUserQuery query, CancellationToken cancellationToken)
    {
        var accounts = await accountRepository.GetAllByUserIdAsync(query.UserId, cancellationToken);

        foreach (var item in accounts)
        {
            item.BrokerCredentials = await brokerAccountCredentialService.GetAsync<BrokerCredentials>(item.Id, cancellationToken);
            var accountTrades = await tradeQueryService.GetTradesByAccountId([item.Id], cancellationToken);
            item.TradesCount = accountTrades.Count();
        }


        return Result<IEnumerable<AccountDto>>.Success(mapper.Map<IEnumerable<AccountDto>>(accounts));
    }
}
