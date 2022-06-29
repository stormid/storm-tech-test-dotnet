using Storm.TechTask.Infrastructure.Clock;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.UnitTests;

using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests
{
    public abstract class BaseIntegrationFixture : BaseFixture
    {
        public static DbContextFactory DbContextFactory { get; private set; } = new DbContextFactory("storm-dotnet-test");

        public BaseIntegrationFixture(ITestOutputHelper output) : base(output)
        {
            // Fresh db on every test run
            DbContextFactory.ResetDb();
        }

        public static IRepository NewRepository() => DbContextFactory.NewRepository();

        public static IClock NewClock() => new Clock();
    }
}
