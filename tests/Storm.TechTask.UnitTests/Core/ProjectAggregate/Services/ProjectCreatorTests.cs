using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Moq;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.Exceptions;
using Storm.TechTask.Core.ProjectAggregate.Services;
using Storm.TechTask.Core.ProjectAggregate.Specifications;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.UnitTests.Utilities.Comparison;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.Services
{
    public class ProjectCreatorTests : BaseFixture
    {
        public ProjectCreatorTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task ReturnsProjectIfNameIsUnique()
        {
            // Arrange
            var mockRepo = new Mock<IReadRepository>(MockBehavior.Strict);
            mockRepo.Setup(r => r.CountAsync(It.IsAny<ProjectsWithNameSpec>(), default)).ReturnsAsync(0);
            var expected = NewProject()
                .Set(p => p.Name, "name")
                .Set(p => p.Category, ProjectCategory.Consultancy)
                .Set(p => p.InternalOnly, false)
                .Set(p => p.Items, new List<ToDoItem>()).Build();
            var creator = new ProjectCreator(mockRepo.Object);

            // Act
            var actual = await creator.Create(expected.Name, expected.Category, expected.InternalOnly, default);

            // Assert
            actual.ShouldHaveSameStateAs(expected);
        }

        [Fact]
        public void ThrowsIfNameIsNotUnique()
        {
            // Arrange
            var mockRepo = new Mock<IReadRepository>(MockBehavior.Strict);
            mockRepo.Setup(r => r.CountAsync(State.SameAs(new ProjectsWithNameSpec("name")), default)).ReturnsAsync(1);
            var creator = new ProjectCreator(mockRepo.Object);

            // Act & Assert
            creator.Invoking(c => c.Create("name", ProjectCategory.Development, false, default)).Should().ThrowAsync<ProjectWithNameAlreadyExistsException>();
        }
    }
}
