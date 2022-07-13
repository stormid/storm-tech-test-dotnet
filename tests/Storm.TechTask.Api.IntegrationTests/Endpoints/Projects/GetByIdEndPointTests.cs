
using IdentityModel.Client;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints.Projects
{
    [Collection("Sequential")]
    public class GetByIdEndPointTests : BaseEndpointFixture
    {
        private readonly HttpClient _client;
        public GetByIdEndPointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
            _client = factory.CreateClient();
        }


        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnGetByProjectId1(AppRole role)
        {
            // Arrange
            Core.ProjectAggregate.Project[]? projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
            HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(role));
            //HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(AppRole.SysAdmin));




            // Act
            HttpResponseMessage? response = await HttpClient.GetAsync($"/Projects/1");



            //    // Assert
            await response.ShouldBeSuccess().WithListPayload(new List<ProjectDto> {
                new ProjectDto(projects[0].Id, projects[0].Name)}.ToList());

        }

    }

    //[Theory]
    //[AllRolesExcept(AppRole.SysAdmin)]
    //public async Task GetById(AppRole role)
    //{
    //    // Arrange
    //    Core.ProjectAggregate.Project[]? projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
    //    HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(role));

    //    // Act
    //    HttpResponseMessage? response = await HttpClient.GetAsync("/Projects");

    //    // Assert
    //    await response.ShouldBeSuccess().WithListPayload(new List<ProjectDto> {
    //        new ProjectDto(projects[0].Id, projects[0].Name),
    //        new ProjectDto(projects[1].Id, projects[1].Name),
    //        new ProjectDto(projects[2].Id, projects[2].Name)});
    //}



    //[Fact]
    //public async Task ShouldBeForbidden()
    //{
    //    // Arrange
    //    Project[]? projects = await Task.WhenAll(NewProject().BuildAndPersist(), NewProject().BuildAndPersist(), NewProject().BuildAndPersist());
    //    HttpClient.SetBearerToken(await TokenIssuer.GetNewToken(AppRole.SysAdmin));

    //    // Act
    //    HttpResponseMessage? response = await HttpClient.GetAsync("/Projects");

    //    // Assert
    //    response.ShouldBeNotAuthorised();
    //}

    //[Fact]
    //public async Task ShouldBeUnauthorized()
    //{
    //    // Act
    //    HttpResponseMessage? response = await HttpClient.GetAsync("/Projects");

    //    // Assert
    //    response.ShouldBeNotAuthenticated();
    //}

}
