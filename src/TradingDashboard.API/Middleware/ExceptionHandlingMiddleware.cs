using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using TradingDashboard.Application.Common.Exceptions;

namespace TradingDashboard.API.Middleware;

public class ExceptionHandlingMiddleware : IExceptionHandler
{
    private readonly ILogger logger;
    private readonly IHostEnvironment environment;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment environment)
    {
        this.logger = logger;
        this.environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpcontext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        var statusCode = GetStatusCode(exception);

        ProblemDetails problemDetails;

        // Special handling for ValidationException to include field-level errors
        if (exception is ValidationException validationException)
        {

            problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "One or more validation failures occurred.",
                Detail = "See the errors property for details.",
                Instance = httpcontext.Request.Path,
                Extensions = new Dictionary<string, object?>
                {
                    { "errors", validationException.Errors }
                }
            };


            logger.LogWarning("Validation failed with errors: {@Errors}", validationException.Errors);
        }
        else
        {
            var title = "";

            if (statusCode == 404)
            {
                title = "Not Found";
            }
            else if (statusCode == 401)
            {
                title = "Unauthtorized";
            }
            else
            {
                title = "An unexpected error occurred.";
                logger.LogError(exception, "An unhandled exception occurred.");
            }



            // Detailed error in development, generic in production
            var detail = environment.IsDevelopment()
                ? exception.Message
                : "An internal error occurred. Please contact support.";


            problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpcontext.Request.Path
            };
        }

        httpcontext.Response.StatusCode = statusCode;
        await httpcontext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        NotSupportedException => StatusCodes.Status400BadRequest,
        NotFoundException => StatusCodes.Status404NotFound,
        AuthenticationException => StatusCodes.Status401Unauthorized,
        ArgumentNullException => StatusCodes.Status400BadRequest,
        ArgumentException => StatusCodes.Status400BadRequest,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };
}