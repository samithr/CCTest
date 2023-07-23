using CCTest.API.ApiResponses;
using Newtonsoft.Json;
using System.Net;

namespace CCTest.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this._next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.Message);
                context.Response.StatusCode = 500;
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new InternalServerErrorResponse(HttpStatusCode.InternalServerError.ToString(), context.Response.StatusCode);
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
