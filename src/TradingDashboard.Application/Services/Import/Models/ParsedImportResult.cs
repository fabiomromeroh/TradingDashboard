namespace TradingDashboard.Application.Services.Import.Models
{
    public record ParsedImportResult(
        IReadOnlyList<RawExecutionRow> Rows,
        IReadOnlyList<string> ParseErrors
        );

}
