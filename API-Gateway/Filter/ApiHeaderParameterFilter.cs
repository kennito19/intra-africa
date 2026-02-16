using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API_Gateway.Filter
{
    public class ApiHeaderParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the "api" header is not already defined in the request headers.
            if (!operation.Parameters.Any(p => p.Name == "api_call" && p.In == ParameterLocation.Header))
            {
                // Add the "api" header with the fixed value "true".
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "api_call",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString("true")
                    }
                });
            }
        }

    }
}
