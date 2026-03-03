using Duende.IdentityModel.Client;

using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class TokenIssuer
    {
        private readonly IdentityServerConfig _identityServerConfig;
        private readonly HttpClient _httpClient;

        public TokenIssuer(IdentityServerConfig identityServerConfig, HttpClient httpClient)
        {
            _identityServerConfig = identityServerConfig;
            _httpClient = httpClient;
        }

        public async Task<DiscoveryDocumentResponse> GetDiscoveryDocument()
        {
            return await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _identityServerConfig.Issuer,
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            });
        }

        public async Task<string> GetNewToken(AppRole roles)
        {
            var disco = await GetDiscoveryDocument();
            Assert.False(disco.IsError, disco.Error);

            var scopes = string.Join(" ", roles.ToScopeNames());

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _identityServerConfig.ClientCredentials.ClientId,
                ClientSecret = _identityServerConfig.ClientCredentials.ClientSecret,
                Scope = scopes
            });

            Assert.NotNull(tokenResponse);
            Assert.False(tokenResponse.IsError, tokenResponse.Error ?? tokenResponse.ErrorDescription);

            return tokenResponse.AccessToken!;
        }
    }
}
