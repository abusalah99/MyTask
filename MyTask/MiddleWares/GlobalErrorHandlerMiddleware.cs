using System;

namespace MyTask;

public class GlobalErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

    public GlobalErrorHandlerMiddleware(ILogger<GlobalErrorHandlerMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            string error = JsonSerializer.Serialize(new { ErrorMessage = exception.Message });
            await context.Response.WriteAsync(error);
        }
    }
}