using MediatR;
using TradingDashboard.Application.Common;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.DeleteImport
{
    public record DeleteImportCommand(Guid importSessionId) : IRequest<Result>
    {
    }
}
