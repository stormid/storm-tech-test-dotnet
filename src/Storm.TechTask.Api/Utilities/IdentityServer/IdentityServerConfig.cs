using Microsoft.Extensions.Configuration;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public class IdentityServerConfig
    {
        public string Issuer { get; set; } = null!;
        public string AuthZEndpoint { get; set; } = null!;
        public string TokenEndpoint { get; set; } = null!;
        public IdentityServerClientConfig ClientCredentials { get; set; }
        public string ApiName { get; set; } = null!;

        public IdentityServerConfig()
        {
            ClientCredentials = new IdentityServerClientConfig();
        }

        public static IdentityServerConfig New(IConfigurationRoot configRoot)
        {
            IdentityServerConfig identityServerConfig = new();
            configRoot.GetSection("IdentityServer").Bind(identityServerConfig);

            return identityServerConfig;
        }
    }
}
