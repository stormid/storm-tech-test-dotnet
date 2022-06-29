using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bogus;

using FluentValidation.TestHelper;

using Storm.TechTask.Core.ProjectAggregate;
using Storm.TechTask.Core.ProjectAggregate.Commands;

using Xunit;

namespace Storm.TechTask.UnitTests.Core.ProjectAggregate.Commands
{
    public class CreateProjectCommandValidatorTests
    {
        private readonly CreateProject.InputValidator _validator;
        private readonly Faker _faker = new();

        public CreateProjectCommandValidatorTests()
        {
            _validator = new CreateProject.InputValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldProduceValidationErrorWhenNameIsNullOrEmpty(string name)
        {
            // Arrange
            var command = new CreateProject.Command(name, ProjectCategory.Development, true);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name is required");
        }

        [Fact]
        public void ShouldProduceValidationErrorWhenNameIsTooLong()
        {
            // Arrange
            var name = _faker.Lorem.Letter(101);
            var command = new CreateProject.Command(name, ProjectCategory.Development, true);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name length must be a maximum of 100 characters");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public void ShouldProduceNoErrorsWhenCommandIsValid(int nameLength)
        {
            // Arrange
            var name = _faker.Lorem.Letter(nameLength);
            var command = new CreateProject.Command(name, ProjectCategory.Development, true);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

}
