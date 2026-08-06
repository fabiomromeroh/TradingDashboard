using AutoMapper;
using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetBrokers
{
    public class GetBrokersQueryHandler : IRequestHandler<GetBrokersQuery, Result<IEnumerable<BrokerDto>>>
    {
        private readonly IBrokerRepository brokerRepository;
        private readonly IMapper mapper;

        public GetBrokersQueryHandler(IBrokerRepository brokerRepository, IMapper mapper)
        {
            this.brokerRepository = brokerRepository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<BrokerDto>>> Handle(GetBrokersQuery getBrokersQuery, CancellationToken ct)
        {
            var brokers = await brokerRepository.GetAllAsync(ct);

            var brokerDto = mapper.Map<IEnumerable<BrokerDto>>(brokers);

            return Result<IEnumerable<BrokerDto>>.Success(brokerDto);
        }
    }
}
