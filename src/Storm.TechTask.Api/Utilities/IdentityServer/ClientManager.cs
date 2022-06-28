using IdentityServer4.Models;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class ClientManager
    {
        public static IEnumerable<Client> Clients(IdentityServerConfig config) =>
            new List<Client>
            {
                new Client
                {
                    ClientName = "Client-Credentials-Client",
                    ClientId = config.ClientCredentials.ClientId,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret(config.ClientCredentials.ClientSecret.Sha256()) },
                    AllowedScopes = ResourceManager.ScopeNames.ToList() // SwaggerUI client is granted all scopes.
                }
            };
    }

}
