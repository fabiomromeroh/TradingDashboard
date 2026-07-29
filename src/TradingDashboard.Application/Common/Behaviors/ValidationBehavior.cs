using FluentValidation;
using MediatR;
using System.Reflection;
using TradingDashboard.Application.Abstractions.Models;

namespace TradingDashboard.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();    // no validators for this request → skip

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (!failures.Any())
            return await next();

        var errors = failures
            .Select(f => new Error(f.PropertyName, f.ErrorMessage))
            .ToList();

        // Safe cast — TResponse is guaranteed to be IResult
        if (typeof(TResponse).IsGenericType)
        {
            var resultType = typeof(TResponse).GetGenericArguments()[0];
            var method = typeof(Result<>)
                            .MakeGenericType(resultType)
                            .GetMethod(nameof(Result<object>.ValidationFailure),
                                BindingFlags.Public | BindingFlags.Static)!;
            return (TResponse)method.Invoke(null, [errors])!;
        }

        return (TResponse)(object)Result.ValidationFailure(errors);
    }
}
