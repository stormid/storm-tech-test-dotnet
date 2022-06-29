using System.Net.Http;
using System.Text;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class BaseEndpointFixture : BaseIntegrationFixture, IClassFixture<CustomWebApplicationFactory>
    {
        protected HttpClient HttpClient { get; private set; }
        protected TokenIssuer TokenIssuer { get; private set; }

        public BaseEndpointFixture(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output)
        {
            HttpClient = factory.CreateClient();
            TokenIssuer = factory.CreateTokenIssuer();
        }
    }

}
