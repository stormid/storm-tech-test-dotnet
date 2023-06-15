using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IdentityModel.Client;

using Moq;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.Core.ProjectAggregate.Commands;
using Storm.TechTask.Core.ProjectAggregate.Exceptions;
using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;
using Storm.TechTask.SharedKernel.Entities;

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
            var projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{projects[0].Id}");

            // Assert
            await response.ShouldBeSuccess().WithObjectPayload(new ProjectDto(projects[0].Id, projects[0].Name));
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnsProjectToDoItems(AppRole role)
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{projects[0].Id}");

            // Assert
            await response.ShouldBeSuccess()
                .WithObjectContainingListPayload(new ProjectDetailsDto(projects[0].Id, projects[0].Name, projects[0].Category, projects[0].Status, projects[0].Items), projects[0].Items);
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnsBadRequestIfIdLessThanOne(AppRole role)
        {
            // Arrange
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync("/Projects/0");

            // Act & Assert
            await response.ShouldBeInvalidCommand();
        }

        [Fact]
        public async Task ShouldBeForbidden()
        {
            // Arrange
            var projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
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
            var response = await this.HttpClient.GetAsync($"/Projects/{It.IsAny<int>()}");

            // Assert
            response.ShouldBeNotAuthenticated();
        }
    }
}
