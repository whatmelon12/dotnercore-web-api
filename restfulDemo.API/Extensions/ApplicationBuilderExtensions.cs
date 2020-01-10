using System;
using Microsoft.AspNetCore.Builder;
using restfulDemo.API.Middleware;

namespace restfulDemo.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRequestResponseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        public static void UseErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
