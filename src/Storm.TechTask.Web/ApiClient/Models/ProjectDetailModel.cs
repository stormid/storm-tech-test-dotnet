namespace Storm.TechTask.Web.ApiClient.Models
{
    public class ProjectDetailModel : ProjectModel
    {
        public string Category{ get; set; }

        public string Status { get; set; }

        public bool InternalOnly { get; set; }

        public ToDoItemModel[] Items { get; set; }
    }
}
