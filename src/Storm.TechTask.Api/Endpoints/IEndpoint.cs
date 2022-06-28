using Microsoft.AspNetCore.Mvc.ModelBinding;

using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Api.Endpoints
{
    public interface IEndpoint
    {
        ILoggingService LoggingService { get; }
        ISecurityService SecurityService { get; }
        ModelStateDictionary ModelState { get; }
    }

}
