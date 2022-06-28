using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    internal class OAuthSecurityRequirementOperationFilter : IOperationFilter
    {
        // This OpFilter class informs Swagger-UI that all endpoints are covered by the "id-svr-4-jwt" OAS security sceheme (defined in AddOAuthOptions() below).
        // The OpFilter is used in in AddOAuthOptions() to add OAuth support to Swagger-UI.
        // Without this, Swagger-UI could not be used to invoke the endpoints (coz it wouldn't inject a bearer token in the http Authorization header).
        // See https://joonasw.net/view/testing-azure-ad-protected-apis-part-1-swagger-ui.
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Get custom attributes on action and controller.
            object[] attributes = context.ApiDescription.CustomAttributes().ToArray();
            if (attributes.OfType<AllowAnonymousAttribute>().Any())
            {
                // Controller/action allows anonymous calls.
                return;
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SwaggerRegistration.SecuritySchemeName
                        },
                        UnresolvedReference = true
                    },
                    new string[]{ }
                }
            });
        }
    }

}
