using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Storm.TechTask.Web.ApiClient;
using Storm.TechTask.Web.ApiClient.Models;

namespace Storm.TechTask.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IProjectApiClient _apiClient;
    private readonly ILogger<IndexModel> _logger;

    public ProjectModel[] Projects { get; set; }

    public IndexModel(IProjectApiClient apiClient, ILogger<IndexModel> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task OnGetAsync()
    {
        Projects = await _apiClient.GetProjects();
    }
}
