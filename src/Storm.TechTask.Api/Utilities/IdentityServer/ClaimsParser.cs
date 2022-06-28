
using System.Security.Claims;

using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class ClaimsParser
    {
        // Call for bitwise-OR set of AppRoles.
        public static string[] ToScopeNames(this AppRole roles)
        {
            var scopeNames = new List<string>();
            foreach (AppRole role in Enum.GetValues(typeof(AppRole)))
            {
                if ((roles & role) > 0)
                {
                    scopeNames.Add(role.ToScopeName());
                }
            }

            return scopeNames.ToArray();
        }

        // Call for single AppRole.
        public static string ToScopeName(this AppRole role) => $"scope.{role}";

        public static IAppUser NewAppUser(ClaimsPrincipal claimsPrincipal)
        {
            // Encode project-specific decision re. which claim holds username e.g. "preferred_username".
            // Currently, IdS4 config is:
            // (1) ResourceOwnerPassword - username is in "sub" claim
            // (2) ClientCredentials - there is no username as yet
            var username = claimsPrincipal.FindFirst("sub")?.Value;
            if (username == null)
            {
                username = "storm.user";
            }

            // Get roles from "scope" claim (could just as easily come from "role" claim, with a different Id Provider config).
            AppRole roles = AppRole.Anonymous;
            foreach (var scope in claimsPrincipal.FindAll("scope"))
            {
                // Eliminate anonymous role we added at start of loop.
                roles &= ~AppRole.Anonymous;

                // Convert scope to role enum. IS4 scope names are "scope.XXX" where "XXX" corresponds to name of App Role (see above).
                roles |= Enum.Parse<AppRole>(scope.Value.Split('.')[1]);
            }

            return new AppUser(username, roles);
        }
    }

}
