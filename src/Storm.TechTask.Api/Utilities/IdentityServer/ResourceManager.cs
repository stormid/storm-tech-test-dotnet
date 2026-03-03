using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public static class ResourceManager
    {
        public static IReadOnlyList<string> ScopeNames { get; }

        static ResourceManager()
        {
            // Create a scope for each AppRole.
            var scopeNames = new List<string>();
            foreach (AppRole role in Enum.GetValues(typeof(AppRole)))
            {
                if (role != AppRole.Anonymous && role != AppRole.MAX)
                {
                    scopeNames.Add(role.ToScopeName());
                }
            }
            ScopeNames = scopeNames.AsReadOnly();
        }
    }
}
