using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IdentityModel.Client;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints.Projects
{
    [Collection("Sequential")]
    public class GetByIdEndpointTests : BaseEndpointFixture
    {
        public GetByIdEndpointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnsProject(AppRole role)
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{projects[0].Id}");

            // Assert
            await response.ShouldBeSuccess().WithObjectPayload(
                new ProjectDetailsDto(projects[0].Id, projects[0].Name, projects[0].Category, projects[0].Status));
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ShouldBeNotFound(AppRole role)
        {
            // Arrange
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/123"); // No need to create any data for this test, we can just enter dummy data

            // Assert
            response.ShouldBeNotFound();
        }

        [Fact]
        public async Task ShouldBeForbidden()
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.SysAdmin));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{projects[0].Id}");

            // Assert
            response.ShouldBeNotAuthorised();
        }

        [Fact]
        public async Task ShouldBeUnauthorized()
        {
            // Act
            var response = await this.HttpClient.GetAsync("/Projects/123"); // No need to create any data for this test, we can just enter dummy data

            // Assert
            response.ShouldBeNotAuthenticated();
        }
    }
}
