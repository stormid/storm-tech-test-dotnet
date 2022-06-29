using System.Linq;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using Storm.TechTask.Infrastructure.Repository;

namespace Storm.TechTask.Api.IntegrationTests
{
    //
    // This class has a static instance in all integration & api tests.
    // It's used to co-ordinate use of a shared in-memory db between test fixtures & code under test.
    //
    public class DbContextFactory
    {
        private readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();
        private readonly string _databaseName;

        public DbContextFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        //
        // Db is reset at start of each integration/api test.
        //
        public void ResetDb()
        {
            NewDbContext().Database.EnsureDeleted();
        }

        //
        // Integration/api tests need a repo for use by entity builders.
        //
        public EfRepository NewRepository()
        {
            return new EfRepository(NewDbContext());
        }

        //
        // Api tests need to eliminate the production dbContext & replace it with one which uses the in-memory db.
        //
        public IServiceCollection ConfigureDbContextForApi(IServiceCollection services)
        {
            // Remove the app's existing ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using an in-memory database for testing.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName, InMemoryDatabaseRoot);
            });

            return services;
        }

        private AppDbContext NewDbContext()
        {
            var options = NewDbContextOptions();
            var mockMediator = new Mock<IMediator>();

            return new AppDbContext(options, mockMediator.Object);
        }

        private DbContextOptions<AppDbContext> NewDbContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase(_databaseName, InMemoryDatabaseRoot)
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }

}
