namespace TradingDashboard.Application.Common.Models
{
    /// <summary>
    /// Represents a paginated result using cursor-based pagination.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated result.</typeparam>
    public record PaginatedResult<T>(
        IEnumerable<T> Items,
        string? NextCursor,

        bool HasMore,
        int? TotalCount = null);
}
