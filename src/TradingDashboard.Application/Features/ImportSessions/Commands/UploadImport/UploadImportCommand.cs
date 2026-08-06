using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.UploadImport
{
    public record UploadImportCommand(
        byte[] FileContent,
        string FileName,
        string BrokerName,
        Guid AccountId) : IRequest<Result<ImportPreviewtDto>>;

}
