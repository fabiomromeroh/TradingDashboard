using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountDto>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<Result<AccountDto>> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(query.Id, cancellationToken);
        if (account is null) return Result<AccountDto>.NotFound(nameof(Domain.Entities.Account), query.Id);

        return Result<AccountDto>.Success(_mapper.Map<AccountDto>(account));
    }
}
