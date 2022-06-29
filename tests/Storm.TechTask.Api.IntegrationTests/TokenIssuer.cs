using System.Threading.Tasks;

using IdentityModel.Client;

using IdentityServer4.Contrib.AspNetCore.Testing.Configuration;
using IdentityServer4.Contrib.AspNetCore.Testing.Services;

using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class TokenIssuer
    {
        private readonly IdentityServerConfig _identityServerConfig;
        private readonly IdentityServerWebHostProxy _identityServerProxy;

        public TokenIssuer(IdentityServerConfig identityServerConfig, IdentityServerWebHostProxy identityServerProxy)
        {
            _identityServerConfig = identityServerConfig;
            _identityServerProxy = identityServerProxy;
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocument()
        {
            return await _identityServerProxy.GetDiscoverResponseAsync();
        }

        public async Task<string> GetNewToken(AppRole roles)
        {
            TokenResponse? tokenResponse;
            tokenResponse = await _identityServerProxy.GetClientAccessTokenAsync(
                new ClientConfiguration(_identityServerConfig.ClientCredentials.ClientId, _identityServerConfig.ClientCredentials.ClientSecret),
                roles.ToScopeNames());
            Assert.NotNull(tokenResponse);
            Assert.False(tokenResponse.IsError, tokenResponse.Error ?? tokenResponse.ErrorDescription);

            return tokenResponse.AccessToken;
        }
    }
}
