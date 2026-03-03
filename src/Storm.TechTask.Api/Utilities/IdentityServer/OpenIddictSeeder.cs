using OpenIddict.Abstractions;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public class OpenIddictSeeder : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityServerConfig _identityServerConfig;

        public OpenIddictSeeder(IServiceScopeFactory scopeFactory, IdentityServerConfig identityServerConfig)
        {
            _scopeFactory = scopeFactory;
            _identityServerConfig = identityServerConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            await SeedScopes(scope.ServiceProvider, cancellationToken);
            await SeedClients(scope.ServiceProvider, _identityServerConfig, cancellationToken);
        }

        public static async Task SeedScopes(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var scopeManager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();

            foreach (var scopeName in ResourceManager.ScopeNames)
            {
                if (await scopeManager.FindByNameAsync(scopeName, cancellationToken) is null)
                {
                    var descriptor = new OpenIddictScopeDescriptor
                    {
                        Name = scopeName
                    };

                    await scopeManager.CreateAsync(descriptor, cancellationToken);
                }
            }
        }

        public static async Task SeedClients(IServiceProvider serviceProvider, IdentityServerConfig config, CancellationToken cancellationToken)
        {
            var appManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await appManager.FindByClientIdAsync(config.ClientCredentials.ClientId, cancellationToken) is null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = config.ClientCredentials.ClientId,
                    ClientSecret = config.ClientCredentials.ClientSecret,
                    DisplayName = "Client-Credentials-Client",
                    ClientType = OpenIddictConstants.ClientTypes.Confidential,
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials
                    }
                };

                foreach (var scopeName in ResourceManager.ScopeNames)
                {
                    descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scopeName);
                }

                await appManager.CreateAsync(descriptor, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
