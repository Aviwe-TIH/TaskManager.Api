namespace TaskManger.Api.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError
        (
            exception,
            "An unhandled exception occurred: {Message}",
            exception.Message
        );
        
        var (status, title, detail) = exception switch
        {
            BadHttpRequestException =>
            (
                StatusCodes.Status400BadRequest,
                "Invalid http request format",
                "Ensure that required parameters are supplied"
            ),
            KeyNotFoundException =>
            (
                StatusCodes.Status404NotFound,
                "Key Not Found",
                "The resource you are looking for does not exist. "
            ),
            _ => 
            (
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "The was an issue handling your request, please try again later"
            )
        };
            
        var instance = httpContext.Request.Path;

        var problemDetails = new ProblemDetails()
        {
            Title = title,
            Detail = detail,
            Status = status,
            Instance = instance
        };

        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}