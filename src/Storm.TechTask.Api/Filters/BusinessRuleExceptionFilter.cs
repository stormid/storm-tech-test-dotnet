using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Api.Filters
{
    public class BusinessRuleExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is BusinessRuleException)
            {
                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Business rule violation",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    Detail = context.Exception.Message
                };
                context.Result = new BadRequestObjectResult(details);
                context.ExceptionHandled = true;
            }
        }
    }

}
