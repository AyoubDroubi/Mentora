using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Mentora.Api.Middleware
{
    /// <summary>
    /// Global exception handler per SRS 9.5
    /// Provides consistent error response format across all endpoints
    /// </summary>
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
            var (statusCode, message, errors) = GetErrorDetails(exception);

            // Log error
            _logger.LogError(
                exception,
                "Exception occurred: {Message} | StatusCode: {StatusCode}",
                message,
                statusCode);

            // Build response per SRS 9.5
            var response = new ErrorResponse
            {
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow,
                StatusCode = statusCode,
                Path = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await httpContext.Response.WriteAsJsonAsync(response, options, cancellationToken);

            return true;
        }

        private (int statusCode, string message, Dictionary<string, string[]>? errors) GetErrorDetails(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status403Forbidden,
                    "Access denied. You don't have permission to perform this action.",
                    null
                ),

                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    exception.Message,
                    null
                ),

                ArgumentException => (
                    StatusCodes.Status400BadRequest,
                    exception.Message,
                    null
                ),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    "The requested resource was not found.",
                    null
                ),

                FluentValidation.ValidationException validationEx => (
                    StatusCodes.Status400BadRequest,
                    "Validation failed. Please check your input.",
                    validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        )
                ),

                HttpRequestException httpEx => (
                    StatusCodes.Status503ServiceUnavailable,
                    "External service is temporarily unavailable. Please try again later.",
                    new Dictionary<string, string[]>
                    {
                        { "service", new[] { httpEx.Message } }
                    }
                ),

                TimeoutException => (
                    StatusCodes.Status504GatewayTimeout,
                    "The request took too long to process. Please try again.",
                    null
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Our team has been notified.",
                    new Dictionary<string, string[]>
                    {
                        { "details", new[] { exception.Message } }
                    }
                )
            };
        }
    }

    /// <summary>
    /// Standard error response format per SRS 9.5
    /// </summary>
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; }
        public DateTime Timestamp { get; set; }
        public int StatusCode { get; set; }
        public string Path { get; set; } = string.Empty;
    }

    /// <summary>
    /// Extension methods for registering exception handler
    /// </summary>
    public static class ExceptionHandlerExtensions
    {
        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            return services;
        }
    }
}
