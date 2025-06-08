using System.Text;

namespace Tasco.Gateway.API.Middlewares
{
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
            // Log the incoming request
            _logger.LogInformation(
                "Request {Method} {Url} received from {IpAddress}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress);

            // Store the original response body stream
            var originalBodyStream = context.Response.Body;

            try
            {
                // Create a new memory stream for the response
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // Continue down the pipeline
                await _next(context);

                // Log the response status and body
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                
                _logger.LogInformation(
                    "Response {StatusCode} for {Method} {Url} sent. Response size: {Size} bytes",
                    context.Response.StatusCode,
                    context.Request.Method,
                    context.Request.Path,
                    responseBodyText.Length);

                // Copy the response to the original stream and restore the stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
    
    // Extension method to register the middleware
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}