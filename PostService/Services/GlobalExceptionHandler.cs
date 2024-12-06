
namespace PostService.Logging
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var error = new
                {
                    Message = "An error occurred while processing your request.",
                    ExceptionMessage = ex.Message
                };

                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
