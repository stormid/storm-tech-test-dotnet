
using Microsoft.AspNetCore.Mvc;

namespace Storm.TechTask.Web.ApiClient
{
    public class ProblemDetailsException<T> : ApplicationException
        where T : ProblemDetails
    {
        public ProblemDetailsException(T problemDetails)
            : base(problemDetails.Detail)
        {
            this.ProblemDetails = problemDetails;
        }

        public T ProblemDetails { get; }

    }
}
