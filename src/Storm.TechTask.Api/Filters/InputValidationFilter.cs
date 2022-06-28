using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Api.Filters
{
    public class InputValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var endpoint = context.Endpoint();
            if (endpoint != null)
            {
                if (context.Exception is InputValidationException exception)
                {
                    endpoint.LoggingService.SecurityLogger.Information("Invalid user input {Error}", FormatConsolidatedErrorMessage(endpoint.ModelState));
                    var details = new ValidationProblemDetails(exception.Errors)
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };

                    context.Result = new BadRequestObjectResult(details);

                    context.ExceptionHandled = true;
                }
            }
        }

        private string FormatConsolidatedErrorMessage(ModelStateDictionary modelState)
        {
            var allErrors = modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return String.Join("|", allErrors.ToArray());
        }
    }
}
