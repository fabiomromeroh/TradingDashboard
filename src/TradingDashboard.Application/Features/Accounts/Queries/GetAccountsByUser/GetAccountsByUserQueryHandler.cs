using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQueryHandler : IRequestHandler<GetAccountsByUserQuery, Result<IEnumerable<AccountDto>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountsByUserQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<AccountDto>>> Handle(GetAccountsByUserQuery query, CancellationToken cancellationToken)
    {
        var accounts = await _accountRepository.GetAllByUserIdAsync(query.UserId, cancellationToken);
        return Result<IEnumerable<AccountDto>>.Success(_mapper.Map<IEnumerable<AccountDto>>(accounts));
    }
}
