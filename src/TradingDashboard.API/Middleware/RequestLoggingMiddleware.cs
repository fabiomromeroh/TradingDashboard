namespace TradingDashboard.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Incoming request: {context.Request.Method} {context.Request.Path}");

        await _next(context);

        Console.WriteLine($"Response status: {context.Response.StatusCode}");

    }
}
