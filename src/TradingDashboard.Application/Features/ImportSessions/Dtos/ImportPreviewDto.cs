namespace TradingDashboard.Application.Features.ImportSessions.Dtos
{
    public record ImportPreviewtDto(
        string FileName,
        string BrokerName,
        Guid AccountId,
        int TotalRows,
        int NewRows,
        int DuplicateRows,
        int InvalidRows,
        IReadOnlyList<PreviewRowDto> Rows   // ← preview included directly
        );

}
