using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.CreateImportSession;

public record CreateImportSessionCommand : IRequest<Result<ImportSessionDto>>
{
    public string FileName { get; init; } = string.Empty;
    public Guid AccountId { get; init; }
}
