using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;

namespace API_Gateway.Filter
{
    public class MeasureResponseTimeOperationFilter : IOperationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MeasureResponseTimeOperationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var sw = Stopwatch.StartNew();
            httpContext.Response.OnCompleted(async (state) =>
            {
                sw.Stop();
                var elapsed = sw.ElapsedMilliseconds;
                operation.Extensions["x-response-time"] = new OpenApiLong(elapsed);

                operation.Responses["200"].Headers["x-response-time"] = new OpenApiHeader
                {
                    Description = "Response time in milliseconds",
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int64"
                    }
                };


                await Task.CompletedTask; // Return a completed Task
            }, null);
        }
    }
}