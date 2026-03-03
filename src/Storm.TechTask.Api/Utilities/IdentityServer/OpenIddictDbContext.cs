using Microsoft.EntityFrameworkCore;

namespace Storm.TechTask.Api.Utilities.IdentityServer
{
    public class OpenIddictDbContext : DbContext
    {
        public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
            : base(options)
        {
        }
    }
}
