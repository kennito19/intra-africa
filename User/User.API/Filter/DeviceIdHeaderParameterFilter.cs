using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace User.API.Filter
{
    public class DeviceIdHeaderParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // Check if the endpoint requires authorization (has [Authorize] attribute)
            bool requiresAuthorization = context.ApiDescription.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute));

            // Add the "DeviceId" parameter only if the endpoint requires authorization
            if (requiresAuthorization)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "device_id",
                    In = ParameterLocation.Header,
                    Description = "Device ID for identification.",
                    Required = true,
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}
