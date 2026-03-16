using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught by middleware.");
            await HandleExceptionAsync(context, ex, _environment.IsDevelopment());
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, bool includeDetails)
    {
        var statusCode = GetStatusCode(exception);
        var problem = new
        {
            title = exception.Message,
            status = statusCode,
            detail = includeDetails ? exception.StackTrace : null,
            traceId = context.TraceIdentifier
        };

        var payload = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(payload);
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        InvalidOperationException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError
    };
}
