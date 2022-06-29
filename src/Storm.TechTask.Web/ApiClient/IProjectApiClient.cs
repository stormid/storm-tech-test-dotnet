using Refit;

using Storm.TechTask.Web.ApiClient.Models;

namespace Storm.TechTask.Web.ApiClient
{
    [Headers("Authorization: Bearer")] 
    public interface IProjectApiClient
    {
        [Get("/Projects")]
        Task<ProjectModel[]> GetProjects();

        [Get("/Projects/{id}")]
        Task<ProjectDetailModel> GetProject(int id);
    }
}
