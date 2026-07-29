using TradingDashboard.Application.Abstractions.Services.Import.Models;

namespace TradingDashboard.Application.Abstractions.Services.FileUpload.Models
{
    public record ParsedImportResult(
        IReadOnlyList<RawExecutionRow> Rows,
        IReadOnlyList<string> ParseErrors
        );

}
