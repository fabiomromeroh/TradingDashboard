using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetBrokers
{
    public class GetBrokersQuery : IRequest<Result<IEnumerable<BrokerDto>>>
    {
    }
}
