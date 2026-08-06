using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetBrokers
{
    public class GetBrokersQuery : IRequest<Result<IEnumerable<BrokerDto>>>
    {
    }
}
