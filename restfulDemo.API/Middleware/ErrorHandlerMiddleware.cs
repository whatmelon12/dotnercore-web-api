using System;
using System.Net;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace restfulDemo.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var errorDetail = new ErrorDetail()
            {
                Message = ex.Message,
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            if (ex is InvalidOperationException) errorDetail.StatusCode = (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetail.StatusCode;
            return context.Response.WriteAsync(errorDetail.ToString());
        }
    }
}
