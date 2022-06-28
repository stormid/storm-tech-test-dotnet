using IdentityServer4.Models;

using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class ResourceManager
    {
        public static IList<string> ScopeNames { get; private set; }

        static ResourceManager()
        {
            // Create a scope for each AppRole.
            ScopeNames = new List<string>();
            foreach (AppRole role in Enum.GetValues(typeof(AppRole)))
            {
                if (role != AppRole.Anonymous && role != AppRole.MAX)
                {
                    ScopeNames.Add(role.ToScopeName());
                }
            }
        }

        public static IEnumerable<ApiScope> Scopes => ScopeNames.Select(sn => new ApiScope(sn));

        public static IEnumerable<ApiResource> Apis(IdentityServerConfig config) =>
            new List<ApiResource>
            {
                new ApiResource {
                    Name = config.ApiName,
                    DisplayName = "StormDotNet API",
                    ApiSecrets = {
                        new Secret(config.ClientCredentials.ClientSecret.Sha256())
                    },
                    Scopes = ScopeNames.ToList()
                }
            };
    }
}
