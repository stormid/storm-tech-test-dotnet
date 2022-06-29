
using Microsoft.AspNetCore.Mvc;

namespace Storm.TechTask.Web.ApiClient
{
    public class ApiClientException : ProblemDetailsException<ProblemDetails>
    {
        public ApiClientException(ProblemDetails problemDetails)
            : base(problemDetails)
        {
        }
    }
}
