using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.ConfirmImport
{
    public record ConfirmImportCommand(
        string FileName,
        string BrokerName,
        Guid AccountId,
        int TotalRows,
        int NewRows,
        int DuplicateRows,
        int InvalidRows,
        IReadOnlyList<PreviewRowDto> Rows
        ) : IRequest<Result<Guid>>;

}
