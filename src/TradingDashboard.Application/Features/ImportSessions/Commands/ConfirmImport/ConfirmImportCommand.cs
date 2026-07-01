using MediatR;
using TradingDashboard.Application.Common;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport
{
    public record ConfirmImportCommand(
        string FileName,
        Guid AccountId,
        int TotalRows,
        int NewRows,
        int DuplicateRows,
        int InvalidRows,
        IReadOnlyList<PreviewRowDto> Rows
        ) : IRequest<Result<Guid>>;

}
