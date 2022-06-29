using System.Threading.Tasks;

using FluentAssertions;

using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.UnitTests.Utilities.Comparison;

namespace Storm.TechTask.Api.IntegrationTests
{
    public static class EntityDbUtils
    {
        public static async Task<T> ShouldBeInDb<T>(this T expected) where T : BaseEntity, IAggregateRoot
        {
            var repository = BaseIntegrationFixture.NewRepository();
            var actual = await repository.GetByIdAsync<T>(expected.Id, default);
            actual.ShouldHaveSameStateAs(expected);

            return expected;
        }
        public static async Task<T> ShouldNotBeInDb<T>(this T expected) where T : BaseEntity, IAggregateRoot
        {
            var repository = BaseIntegrationFixture.NewRepository();
            var actual = await repository.GetByIdAsync<T>(expected.Id, default);
            actual.Should().BeNull();

            return expected;
        }
    }
}
