using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

using Serilog.Context;

using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.Infrastructure.Logging;
using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.Api.Filters
{
    public class AuthenticationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.Endpoint();
            if (endpoint != null)
            {
                ClaimsPrincipal claimsPrincipal = context.HttpContext.User;
                if ((claimsPrincipal != null) && (claimsPrincipal.Identity != null) && claimsPrincipal.Identity.IsAuthenticated)
                {
                    var appUser = ClaimsParser.NewAppUser(claimsPrincipal);
                    SetLoggingContext(appUser);
                    endpoint.LoggingService.SecurityLogger.Debug("Received request from authenticated user {Username}", appUser.Username);
                    endpoint.SecurityService.CurrentUser = appUser;
                }
                else if (context.AttributeExistsOnEndpoint<AllowAnonymousAttribute>())
                {
                    endpoint.LoggingService.SecurityLogger.Debug("Received request from unauthenticated user, but endpoint accepts anonymous access");
                    endpoint.SecurityService.CurrentUser = AppUser.Anonymous();
                }
                else
                {
                    endpoint.LoggingService.SecurityLogger.Warning("Received request from unauthenticated user");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private void SetLoggingContext(IAppUser user)
        {
            LogContext.PushProperty(SerilogConfig.PropNameUsername, user.Username);
        }
    }
}
