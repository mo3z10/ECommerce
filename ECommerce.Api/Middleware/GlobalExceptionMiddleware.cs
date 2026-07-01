using System.Net;
using System.Text.Json;
namespace ECommerce.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);

            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            switch (ex)
            {
                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = ex.Message;
                    break;

                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = ex.Message;
                    break;

                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;

                case InvalidOperationException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
}
