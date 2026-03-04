using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class SwaggerRegistration
    {
        public const string SecuritySchemeName = "id-svr-4-jwt";

        public static SwaggerGenOptions AddOAuthOptions(this SwaggerGenOptions options, IConfigurationRoot configRoot)
        {
            var identityServerConfig = IdentityServerConfig.New(configRoot);
            var scopes = ResourceManager.ScopeNames.ToDictionary(s => s, s => s);
            var scheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(identityServerConfig.AuthZEndpoint),
                        TokenUrl = new Uri(identityServerConfig.TokenEndpoint),
                        Scopes = scopes
                    }
                }
            };
            options.AddSecurityDefinition(SecuritySchemeName, scheme);
            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference(SecuritySchemeName, doc), scopes.Keys.ToList() }
            });

            return options;
        }

        public static SwaggerUIOptions UseOAuthOptions(this SwaggerUIOptions options, IConfigurationRoot configRoot)
        {
            var identityServerConfig = IdentityServerConfig.New(configRoot);
            options.OAuthClientId(identityServerConfig.ClientCredentials.ClientId);
            options.OAuthClientSecret(identityServerConfig.ClientCredentials.ClientSecret);
            return options;
        }
    }

}
