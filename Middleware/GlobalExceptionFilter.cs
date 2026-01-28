using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using test_dotnet.DTOs.Responses;

namespace test_dotnet.Middleware;

/// <summary>
/// Global exception handler - catches ALL unhandled exceptions
/// Returns consistent error responses instead of ugly stack traces
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Unhandled exception occurred");

        var response = ApiResponse.ErrorResponse(
            "An error occurred while processing your request"
        );

        // In development, include the actual error details
        if (_env.IsDevelopment())
        {
            response.Errors = new List<string>
            {
                context.Exception.Message,
                context.Exception.StackTrace ?? ""
            };
        }

        context.Result = new ObjectResult(response)
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}
