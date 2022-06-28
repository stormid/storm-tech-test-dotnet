namespace Storm.TechTask.SharedKernel.Authorization
{
    public class AppUser : IAppUser
    {
        public AppUser(string username, AppRole roles)
        {
            Username = username;
            Roles = roles;
        }

        public string Username { get; protected set; }
        public AppRole Roles { get; protected set; }

        public static IAppUser Anonymous() => new AppUser(string.Empty, AppRole.Anonymous);

        bool IAppUser.HasRole(AppRole role)
        {
            return (Roles & role) > 0;
        }
    }
}
