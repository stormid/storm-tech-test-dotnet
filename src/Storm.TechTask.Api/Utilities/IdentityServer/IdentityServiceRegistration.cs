using System.Security.Claims;

using Microsoft.EntityFrameworkCore;

using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdProvider(this IServiceCollection services, IWebHostEnvironment environment, IConfigurationRoot configRoot)
        {
            // We can't setup Identity Server this way when it's hosted in TestServer, so do nothing if we're in the apitest env.
            if (environment.EnvironmentName != SharedKernel.Utilities.Environment.ApiTest)
            {
                var identityServerConfig = IdentityServerConfig.New(configRoot);
                services.AddSingleton(identityServerConfig);

                AddOpenIddictServices(services);

                services.AddHostedService<OpenIddictSeeder>();

                services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
            }
        }

        public static void AddTestIdProvider(this IServiceCollection services, IdentityServerConfig identityServerConfig)
        {
            // IdentityServerConfig is not registered in DI for tests; seeding is performed
            // explicitly in CustomWebApplicationFactory.CreateHost, not via IHostedService.
            AddOpenIddictServices(services);

            services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        private static void AddOpenIddictServices(IServiceCollection services)
        {
            // Manual registration avoids EF Core 9+ InMemory/SQLite provider conflict.
            services.AddScoped(sp =>
            {
                var options = new DbContextOptionsBuilder<OpenIddictDbContext>()
                    .UseInMemoryDatabase("OpenIddict")
                    .UseOpenIddict()
                    .Options;
                return new OpenIddictDbContext(options);
            });

            services.AddOpenIddict()
                .AddCore(options
                    => options.UseEntityFrameworkCore().UseDbContext<OpenIddictDbContext>())
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("connect/token");

                    options.AllowClientCredentialsFlow();

                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.DisableAccessTokenEncryption();

                    options.AddEventHandler<OpenIddictServerEvents.HandleTokenRequestContext>(builder =>
                        builder.UseInlineHandler(context =>
                        {
                            var identity = new ClaimsIdentity(
                                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                            identity.SetClaim(OpenIddictConstants.Claims.Subject, context.Request.ClientId);
                            identity.SetScopes(context.Request.GetScopes());
                            context.SignIn(new ClaimsPrincipal(identity));
                            return default;
                        }));

                    options
                        .UseAspNetCore()
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}
