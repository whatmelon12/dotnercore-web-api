using System;
using System.IO;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;

namespace restfulDemo.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await context.Request.Body.CopyToAsync(requestStream);
                _logger.LogInfo("Http request information: " +
                    $"Schema: {context.Request.Scheme} " +
                    $"Host: {context.Request.Host} " +
                    $"Method: {context.Request.Method} " +
                    $"Path: {context.Request.Path} " +
                    $"QueryString: {context.Request.QueryString} " +
                    $"Request Body: {ReadStreamInChunks(requestStream)}");
            }
            
            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            await using (var responseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                responseBody.Seek(0, SeekOrigin.Begin);

                _logger.LogInfo("Http response information: " +
                    $"Schema: {context.Request.Scheme} " +
                    $"Host: {context.Request.Host} " +
                    $"Method: {context.Request.Method} " +
                    $"Path: {context.Request.Path} " +
                    $"QueryString: {context.Request.QueryString} " +
                    $"StatusCode: {context.Response.StatusCode} " +
                    $"Response Body: {ReadStreamInChunks(responseBody)}");
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            string result;

            stream.Seek(0, SeekOrigin.Begin);

            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[readChunkBufferLength];
                int readChunkLength;

                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }
    }
}
