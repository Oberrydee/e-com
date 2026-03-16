using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problem = new
        {
            title = exception.Message,
            status = GetStatusCode(exception),
            detail = exception is null ? null : exception.StackTrace
        };

        var payload = JsonSerializer.Serialize(problem);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(exception);

        return context.Response.WriteAsync(payload);
    }

    private static int GetStatusCode(Exception exception) => exception switch
    {
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        InvalidOperationException => (int)HttpStatusCode.BadRequest,
        _ => (int)HttpStatusCode.InternalServerError
    };
}
