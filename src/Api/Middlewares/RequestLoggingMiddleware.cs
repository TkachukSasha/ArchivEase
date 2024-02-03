using Serilog.Context;

namespace Api.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate _next)
{
    public Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            return _next(context);
        }
    }
}
