using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.SharedKernel.Interfaces
{
    public interface ISecurityService
    {
        IAppUser CurrentUser { get; set; }
    }
}
