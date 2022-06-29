using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IdentityServer4.Contrib.AspNetCore.Testing.Builder;
using IdentityServer4.Contrib.AspNetCore.Testing.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;

using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.SharedKernel;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IdentityServerConfig _identityServerConfig;
        private readonly IdentityServerWebHostProxy _identityServerProxy;

        public CustomWebApplicationFactory()
        {
            _identityServerConfig = GetIdentityServerConfig();

            var webHostBuilder = new IdentityServerTestWebHostBuilder()
                .AddApiScopes(ResourceManager.Scopes.ToArray())
                .AddApiResources(ResourceManager.Apis(_identityServerConfig).ToArray())
                .AddClients(ClientManager.Clients(_identityServerConfig).ToArray())
                .CreateWebHostBuilder();
            _identityServerProxy = new IdentityServerWebHostProxy(webHostBuilder);
        }

        private IdentityServerConfig GetIdentityServerConfig()
        {
            // Read config from Channel.Api settings, then overwrite URLs to eliminate dev port numbers.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "../../../../../src/Storm.TechTask.Api"))
                .AddJsonFile("appsettings.json");
            var identityServerConfig = IdentityServerConfig.New(builder.Build());
            identityServerConfig.Issuer = "http://localhost";
            identityServerConfig.AuthZEndpoint = "http://localhost/connect/authorize";
            identityServerConfig.TokenEndpoint = "http://localhost/connect/token";

            return identityServerConfig;
        }

        public TokenIssuer CreateTokenIssuer() => new TokenIssuer(_identityServerConfig, _identityServerProxy);

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("src/StormDotNet.Channel.Api")
                .ConfigureServices(services =>
                {
                    BaseEndpointFixture.DbContextFactory.ConfigureDbContextForApi(services);

                    services.AddTestIdProvider(_identityServerConfig, _identityServerProxy.IdentityServer.CreateHandler());
                    services.AddSharedKernel();
                });
        }

        /// <summary>
        /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
        /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment(SharedKernel.Utilities.Environment.ApiTest);
            var host = builder.Build();

            host.Start();
            return host;
        }
    }

    public class CustomWebApplicationFactory : CustomWebApplicationFactory<Program>
    {
    }
}
