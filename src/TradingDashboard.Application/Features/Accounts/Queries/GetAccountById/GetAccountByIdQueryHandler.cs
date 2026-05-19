using AutoMapper;
using MediatR;
using TradingDashboard.Application.Common.Exceptions;
using TradingDashboard.Application.Common.Interfaces;
using TradingDashboard.Application.Features.Accounts.Dtos;

namespace TradingDashboard.Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.Account), query.Id);

        return _mapper.Map<AccountDto>(account);
    }
}
