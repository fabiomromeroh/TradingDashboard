using FluentValidation.Results;

namespace TradingDashboard.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(List<ValidationFailure> failures)
        : base("One or more validation failures occurred.")
    {
        Errors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray()
            );
    }

    public IDictionary<string, string[]> Errors { get; }
}
