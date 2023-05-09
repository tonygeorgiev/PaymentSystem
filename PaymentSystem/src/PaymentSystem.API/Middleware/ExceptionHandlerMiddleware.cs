using PaymentSystem.API.Common.Exceotions;
using PaymentSystem.API.Models;
using PaymentSystem.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace PaymentSystem.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly Dictionary<Type, HttpStatusCode> _exceptionHandlers = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(ArgumentException), HttpStatusCode.BadRequest },
                { typeof(NotFoundException), HttpStatusCode.NotFound },
                { typeof(EntityNotFoundException), HttpStatusCode.NotFound },
                { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
                { typeof(InvalidOperationException), HttpStatusCode.BadRequest }
            };

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred.");

                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "InternalServerError",
                    ErrorMessage = "An internal server error has occurred."
                };

                var statusCode = HttpStatusCode.InternalServerError;

                var exceptionType = ex.GetType();

                if (_exceptionHandlers.ContainsKey(exceptionType))
                {
                    statusCode = _exceptionHandlers[exceptionType];
                    errorResponse.ErrorCode = exceptionType.Name;
                    errorResponse.ErrorMessage = ex.Message;
                }

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)statusCode;

                await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
