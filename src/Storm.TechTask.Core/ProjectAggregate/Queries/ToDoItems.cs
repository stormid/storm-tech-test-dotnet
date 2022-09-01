
using FluentValidation;

using MediatR;

using Storm.TechTask.Core.ProjectAggregate.Specifications;
using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Handlers;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.Queries
{
    public static class ToDoItems
    {
        public class Query : IQuery<List<ToDoItem>>
        {
            public int ProjectId { get; set; }
            public Query(int projectId)
            {
                this.ProjectId = projectId;
            }
        }

        public class InputValidator : AbstractValidator<Query>
        {
            public InputValidator()
            {
                RuleFor(x => x.ProjectId)
                    .GreaterThan(0).WithMessage("Project ID must be greater than zero");
            }
        }

        public class Authorizer : BaseQueryAuthorizer<Query>
        {
            protected override bool UserHasCorrectRole(IAppUser user) => user.HasRole(AppRole.ProjectAdmin | AppRole.ProjectReader);
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Query, List<ToDoItem>>
        {
            public Handler(IRepository repository, IUserSession session) : base(repository) { }

            public async Task<List<ToDoItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await LoadEntitiesBySpec(new ToDoItemByProjectIdSpec(request.ProjectId), cancellationToken);
            }
        }
    }
}
