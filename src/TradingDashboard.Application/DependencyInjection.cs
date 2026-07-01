using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TradingDashboard.Application.Common.Behaviors;

namespace TradingDashboard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // --- MediatR ---
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Scans assembly and registers ALL AbstractValidator<T> implementations
        // This picks up CreateTradeCommandValidator automatically
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Registers the pipeline behavior that runs validators automatically
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // --- AutoMapper ---
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));



        return services;
    }
}
