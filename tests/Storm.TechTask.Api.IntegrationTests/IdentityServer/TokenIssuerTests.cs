using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.IdentityServer
{
    [Collection("Sequential")]
    public class TokenIssuerTests : BaseEndpointFixture
    {
        public TokenIssuerTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }

        [Fact]
        public async Task ReturnsDiscoveryDocument()
        {
            // Arrange

            // Act
            var discoveryDocumentResponse = await this.TokenIssuer.GetDiscoveryDocument();

            // Assert
            discoveryDocumentResponse.Should().NotBeNull();
            discoveryDocumentResponse.IsError.Should().BeFalse(discoveryDocumentResponse.Error);
        }

        [Fact]
        public async Task ReturnsToken()
        {
            // Arrange

            // Acr
            var token = await this.TokenIssuer.GetNewToken(AppRole.SysAdmin);

            // Assert
            token.Should().NotBeNull();
        }
    }
}
