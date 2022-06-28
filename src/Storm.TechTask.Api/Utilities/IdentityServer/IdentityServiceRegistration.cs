using IdentityServer4.AccessTokenValidation;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdProvider(this IServiceCollection services, IWebHostEnvironment environment, IConfigurationRoot configRoot)
        {
            // We can't setup Identity Server this way when it's hosted in TestServer, so do nothing if we're in the apitest env.
            if (environment.EnvironmentName != SharedKernel.Utilities.Environment.ApiTest)
            {
                // Read config
                var identityServerConfig = IdentityServerConfig.New(configRoot);

                // Token issue
                services.AddIdentityServer()
                        .AddDeveloperSigningCredential()
                        .AddInMemoryApiScopes(ResourceManager.Scopes)
                        .AddInMemoryApiResources(ResourceManager.Apis(identityServerConfig))
                        .AddInMemoryClients(ClientManager.Clients(identityServerConfig));

                // Token validation
                services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                        .AddIdentityServerAuthentication(options =>
                        {
                            options.Authority = identityServerConfig.Issuer;
                            options.RequireHttpsMetadata = false;
                            options.ApiName = identityServerConfig.ApiName;
                        });
            }
        }

        public static void UseIdProvider(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // We can't start Identity Server this way when it's hosted in TestServer, so do nothing if we're in the apitest env.
            if (environment.EnvironmentName != SharedKernel.Utilities.Environment.ApiTest)
            {
                // Read config
                app.UseIdentityServer();
            }
        }

    }

}
