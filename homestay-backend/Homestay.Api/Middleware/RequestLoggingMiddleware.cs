namespace Homestay.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var requestMethod = context.Request.Method;
        var requestPath = context.Request.Path;
        var userEmail = context.User.Identity?.IsAuthenticated == true 
            ? context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
            : "Anonymous";

        _logger.LogInformation(
            "Incoming Request: {Method} {Path} by {User}", 
            requestMethod, requestPath, userEmail);

        await _next(context);

        var duration = DateTime.UtcNow - startTime;
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation(
            "Completed Request: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms", 
            requestMethod, requestPath, statusCode, duration.TotalMilliseconds);
    }
}
