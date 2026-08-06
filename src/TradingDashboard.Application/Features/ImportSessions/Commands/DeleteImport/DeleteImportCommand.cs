using MediatR;
using TradingDashboard.Application.Common.Models;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.DeleteImport
{
    public record DeleteImportCommand(Guid importSessionId) : IRequest<Result>
    {
    }
}
