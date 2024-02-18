using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using ResponseWrapper.Model;


namespace ResponseWrapping.API.Middleware
{
    public class ResponseWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBody = context.Response.Body;
            try
            {
                string responseBody = null;

                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;
                    await _next(context);
                    memStream.Position = 0;
                    responseBody = new StreamReader(memStream).ReadToEnd();
                }

                var data = JsonConvert.DeserializeObject(responseBody);

                var wrapperModel = new ResponseWrapperModel(context.Response.StatusCode)
                {
                    RequestUrl = context.Request.GetDisplayUrl(),
                    Data = data,
                    StatusCode = context.Response.StatusCode
                };

                var json = JsonConvert.SerializeObject(wrapperModel);

                var buffer = Encoding.UTF8.GetBytes(json);
                using (var output = new MemoryStream(buffer))
                {
                    await output.CopyToAsync(originalBody);
                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }
    }
}
