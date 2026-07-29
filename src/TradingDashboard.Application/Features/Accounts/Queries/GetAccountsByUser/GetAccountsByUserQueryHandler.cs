using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.BrokerSync;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQueryHandler : IRequestHandler<GetAccountsByUserQuery, Result<IEnumerable<AccountDto>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IBrokerAccountCredentialService _brokerAccountCredentialService;
    private readonly ITradeRepository _tradeRepository;

    public GetAccountsByUserQueryHandler(IAccountRepository accountRepository,
        IBrokerAccountCredentialService brokerAccountCredentialService,
        ITradeRepository tradeRepository,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _brokerAccountCredentialService = brokerAccountCredentialService;
        _tradeRepository = tradeRepository;
    }

    public async Task<Result<IEnumerable<AccountDto>>> Handle(GetAccountsByUserQuery query, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllByUserIdAsync(query.UserId, cancellationToken);

        foreach (var item in accounts)
        {
            item.BrokerCredentials = await _brokerAccountCredentialService.GetAsync<BrokerCredentials>(item.Id, cancellationToken);
            var accountTrades = await _tradeRepository.GetTradesByAccountId([item.Id], cancellationToken);
            item.TradesCount = accountTrades.Count();
        }


        return Result<IEnumerable<AccountDto>>.Success(_mapper.Map<IEnumerable<AccountDto>>(accounts));
    }
}
