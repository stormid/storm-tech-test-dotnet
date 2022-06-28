
using FluentValidation;

using MediatR;

using Storm.TechTask.Core.ProjectAggregate.Specifications;
using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Handlers;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.Queries
{
    public static class ProjectDetails
    {
        public class Query : IQuery<Project?>
        {
            public int Id { get; set; }

            public Query(int id)
            {
                this.Id = id;
            }

            public Query() : this(0)
            {
            }
        }

        public class InputValidator : AbstractValidator<Query>
        {
            public InputValidator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("ID must be greater than zero");
            }
        }

        public class Authorizer : BaseQueryAuthorizer<Query>
        {
            protected override bool UserHasCorrectRole(IAppUser user) => user.HasRole(AppRole.ProjectAdmin | AppRole.ProjectReader);
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Query, Project?>
        {
            public Handler(IRepository repository) : base(repository) { }

            public async Task<Project?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await LoadEntityBySpec<Project, ProjectByIdSpec>(new ProjectByIdSpec(request.Id), cancellationToken);
            }
        }
    }
}
