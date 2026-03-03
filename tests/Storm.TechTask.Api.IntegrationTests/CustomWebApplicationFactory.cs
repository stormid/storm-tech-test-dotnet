using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.SharedKernel;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IdentityServerConfig _identityServerConfig;

        public CustomWebApplicationFactory()
        {
            _identityServerConfig = GetIdentityServerConfig();
        }

        private IdentityServerConfig GetIdentityServerConfig()
        {
            // Read config from Api settings, then overwrite URLs to eliminate dev port numbers.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory, "../../../../../src/Storm.TechTask.Api"))
                .AddJsonFile("appsettings.json");
            var identityServerConfig = IdentityServerConfig.New(builder.Build());
            identityServerConfig.Issuer = "http://localhost";
            identityServerConfig.AuthZEndpoint = "http://localhost/connect/authorize";
            identityServerConfig.TokenEndpoint = "http://localhost/connect/token";

            return identityServerConfig;
        }

        public TokenIssuer CreateTokenIssuer() => new TokenIssuer(_identityServerConfig, this.CreateClient());

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureServices(services =>
                {
                    BaseEndpointFixture.DbContextFactory.ConfigureDbContextForApi(services);

                    services.AddTestIdProvider(_identityServerConfig);
                    services.AddSharedKernel();
                });
        }

        /// <summary>
        /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
        /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
        /// </summary>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(SharedKernel.Utilities.Environment.ApiTest);
            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OpenIddictDbContext>();
                db.Database.EnsureCreated();

                OpenIddictSeeder.SeedScopes(scope.ServiceProvider, CancellationToken.None).GetAwaiter().GetResult();
                OpenIddictSeeder.SeedClients(scope.ServiceProvider, _identityServerConfig, CancellationToken.None).GetAwaiter().GetResult();
            }

            host.Start();
            return host;
        }
    }

    public class CustomWebApplicationFactory : CustomWebApplicationFactory<Program>
    {
    }
}
