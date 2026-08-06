using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.SyncBrokerImport
{
    public record SyncBrokerImportCommand(Guid AccountId) : IRequest<Result<SyncBrokerDto>>;

}
