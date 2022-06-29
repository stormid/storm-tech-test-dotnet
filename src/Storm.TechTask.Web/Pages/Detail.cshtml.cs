using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Storm.TechTask.Web.ApiClient;
using Storm.TechTask.Web.ApiClient.Models;

namespace Storm.TechTask.Web.Pages
{
    public class DetailModel : PageModel
    {
        private readonly IProjectApiClient _apiClient;
        private readonly ILogger<DetailModel> _logger;

        public ProjectDetailModel Project { get; set; }

        public DetailModel(IProjectApiClient apiClient, ILogger<DetailModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task OnGetAsync(int id)
        {
            Project = await _apiClient.GetProject(id);
        }
    }
}
