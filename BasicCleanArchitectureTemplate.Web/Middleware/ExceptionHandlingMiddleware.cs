using System.Net;
using BasicCleanArchitectureTemplate.Core.Exceptions;
using Newtonsoft.Json;

namespace BasicCleanArchitectureTemplate.Web.Middleware
{
    public static class ExceptionHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _nextRequestDelegate;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate nextRequestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _nextRequestDelegate = nextRequestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _nextRequestDelegate(httpContext);
            }
            catch (EntityNotFoundException exception)
            {
                await CreateErrorResponse(httpContext, exception, HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                await CreateErrorResponse(httpContext, exception);
            }
        }

        private async Task CreateErrorResponse(
            HttpContext httpContext,
            Exception exception,
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            var response = httpContext.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            _logger.LogError(exception, "Middleware error.");

            await response.WriteAsync(JsonConvert.SerializeObject(new { exception.Message }));
        }
    }
}
