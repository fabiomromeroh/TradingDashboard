using MediatR;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.CreateImportSession;

public record CreateImportSessionCommand : IRequest<ImportSessionDto>
{
    public string FileName { get; init; } = string.Empty;
    public Guid AccountId { get; init; }
}
