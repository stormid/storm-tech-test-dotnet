using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using FluentValidation.TestHelper;

using Storm.TechTask.Core.ProjectAggregate.Queries;

using Xunit;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.Queries
{
    public class ProjectByIdQueryValidatorTests
    {
        private readonly Faker _faker = new();
        private readonly ProjectDetails.InputValidator _validator;

        public ProjectByIdQueryValidatorTests()
        {
            _validator = new ProjectDetails.InputValidator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ShouldProduceValidationErrorWhenIdIsNotGreaterThanZero(int id)
        {
            // Arrange
            string? name = _faker.Lorem.Letter(100);
            var query = new ProjectDetails.Query(id);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("ID must be greater than zero");
        }

        [Fact]
        public void ShouldProduceNoErrorsWhenCommandIsValid()
        {
            // Arrange
            var query = new ProjectDetails.Query(1);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

