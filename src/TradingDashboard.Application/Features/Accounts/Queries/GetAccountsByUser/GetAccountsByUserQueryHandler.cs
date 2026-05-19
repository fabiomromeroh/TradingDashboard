using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQueryHandler : IRequestHandler<GetAccountsByUserQuery, IEnumerable<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountsByUserQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAccountsByUserQuery query, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllByUserIdAsync(query.UserId, cancellationToken);
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}
