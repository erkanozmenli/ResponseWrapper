

using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using ResponseWrapper.Model;
using System.Net;

namespace ResponseWrapper.API.Middleware
{
    public class ExceptionResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger _logger;

        public ExceptionResponseWrapperMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionResponseWrapperMiddleware> logger)
        {
            _next = next;
            _env = env;
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
                _logger.LogError($"{ex}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var wrapperModel = new ResponseWrapperModel(context.Response.StatusCode)
                {
                    RequestUrl = context.Request.GetDisplayUrl(),
                    StatusCode = context.Response.StatusCode,
                    ErrorDetails = _env.IsDevelopment() ? ex.ToString() : ex.Message
                };
                var json = JsonConvert.SerializeObject(wrapperModel);
                await context.Response.WriteAsync(json);
            }
        }

    }
}
