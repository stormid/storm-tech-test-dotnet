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
    public class ListEndpointTests : BaseEndpointFixture
    {
        public ListEndpointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnsProjects(AppRole role)
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync("/Projects");

            // Assert
            await response.ShouldBeSuccess().WithListPayload(new List<ProjectDto> {
                new ProjectDto(projects[0].Id, projects[0].Name),
                new ProjectDto(projects[1].Id, projects[1].Name),
                new ProjectDto(projects[2].Id, projects[2].Name)});
        }

        [Fact]
        public async Task ShouldBeForbidden()
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.SysAdmin));

            // Act
            var response = await this.HttpClient.GetAsync("/Projects");

            // Assert
            response.ShouldBeNotAuthorised();
        }

        [Fact]
        public async Task ShouldBeUnauthorized()
        {
            // Act
            var response = await this.HttpClient.GetAsync("/Projects");

            // Assert
            response.ShouldBeNotAuthenticated();
        }
    }
}
