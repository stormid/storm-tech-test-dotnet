using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Storm.TechTask.Api.Endpoints;

namespace Storm.TechTask.Api.Filters
{
    public static class FilterContextUtils
    {
        public static IEndpoint? Endpoint(this ActionExecutingContext actionContext)
        {
            return actionContext.Controller as IEndpoint;
        }

        public static IEndpoint? Endpoint(this ActionExecutedContext actionContext)
        {
            return actionContext.Controller as IEndpoint;
        }

        public static bool AttributeExistsOnEndpoint<TAttribute>(this ActionContext actionContext)
        {
            // For why this implementation is used, see here: 
            // https://stackoverflow.com/questions/31874733/how-to-read-action-methods-attributes-in-asp-net-core-mvc/60602828#60602828
            return actionContext.ActionDescriptor.EndpointMetadata.OfType<TAttribute>().Any();
        }
    }

}
