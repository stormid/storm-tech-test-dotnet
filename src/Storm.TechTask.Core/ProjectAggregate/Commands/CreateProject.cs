using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Storm.TechTask.Core.ProjectAggregate.Services;
using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Handlers;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.SharedKernel.Validation;

namespace Storm.TechTask.Core.ProjectAggregate.Commands
{
    public static class CreateProject
    {
        public class Command : ICommand<Project>
        {
            [NullMessage("Name is required")]
            public string Name { get; set; } = null!;
            public ProjectCategory Category { get; set; }
            public bool InternalOnly { get; set; }

            public Command(string name, ProjectCategory category, bool internalOnly)
            {
                this.Name = name;
                this.Category = category;
                this.InternalOnly = internalOnly;
            }

            public Command() : this(String.Empty, ProjectCategory.NotSet, false)
            {
            }
        }

        public class InputValidator : AbstractValidator<Command>
        {
            public InputValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .MaximumLength(100).WithMessage("Name length must be a maximum of 100 characters");

                RuleFor(x => x.Category)
                    .NotNull().WithMessage("Category is required");
            }
        }

        public class Authorizer : BaseCommandAuthorizer<Command>
        {
            public Authorizer(IRepository repository) : base(repository) { }

            protected override bool UserHasCorrectRole(IAppUser user) => user.HasRole(AppRole.ProjectAdmin);
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, Project>
        {
            public Handler(IRepository repository) : base(repository)
            {
            }

            public async Task<Project> Handle(Command request, CancellationToken cancellationToken)
            {
                //Guard.Against.NullOrEmpty(request.Name, nameof(request.Name));
                var project = await new ProjectCreator(_repository).Create(request.Name, request.Category, request.InternalOnly, cancellationToken);

                return await CreateEntity(project, cancellationToken);
            }
        }
    }
}
