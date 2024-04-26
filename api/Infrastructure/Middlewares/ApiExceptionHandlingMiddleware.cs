using api.Domain;
using api.Infrastructure.ProblemDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Infrastructure.Middlewares
{
    public class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

        public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            string result;
            string requestBody = string.Empty;
            if (context.Request.Method == "POST")
            {
                requestBody = await GetRequestBodyAsync(context.Request);
            }
            if (ex is DomainException e)
            {
                var problemDetails = new CustomValidationProblemDetails(new List<ValidationError> { new() { Code = e.Code, Message = e.Message } })
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(problemDetails);
            }
            else if (ex is ArgumentException e1)
            {
                var problemDetails = new CustomValidationProblemDetails(new List<ValidationError> { new() { Message = e1.Message } })
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(problemDetails);
            }
            else if (ex is ArgumentNullException e2)
            {
                var problemDetails = new CustomValidationProblemDetails(new List<ValidationError> { new() { Message = e2.Message } })
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(problemDetails);
            }
            else if (ex is FormatException e3)
            {
                var problemDetails = new CustomValidationProblemDetails(new List<ValidationError> { new() { Message = e3.Message } })
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Instance = context.Request.Path,
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(problemDetails);
            }
            else
            {
                _logger.LogError(ex, $"An unhandled exception has occurred, {ex.Message}{(context.Request.Method == "POST" ? $"\nrequestBody:\n{requestBody}" : "")}");
                var problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error.",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = context.Request.Path,
                    Detail = "Internal server error occurred!"
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result = JsonSerializer.Serialize(problemDetails);
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
        static async Task<string> GetRequestBodyAsync(HttpRequest request,
                                                     Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            var body = "";
            if (request.ContentLength == null || !(request.ContentLength > 0) || !request.Body.CanSeek) return body;

            request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(request.Body, encoding, true, 1024, true))
                body = await reader.ReadToEndAsync();

            request.Body.Position = 0;
            return body;
        }

    }
}