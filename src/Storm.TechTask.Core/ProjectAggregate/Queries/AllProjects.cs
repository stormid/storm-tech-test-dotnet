using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Handlers;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.Queries
{
    public static class AllProjects
    {
        public class Query : IQuery<List<Project>>
        {
        }

        public class Authorizer : BaseQueryAuthorizer<Query>
        {
            protected override bool UserHasCorrectRole(IAppUser user) => user.HasRole(AppRole.ProjectAdmin | AppRole.ProjectReader);
        }

        public class Handler : BaseQueryHandler, IRequestHandler<Query, List<Project>>
        {
            public Handler(IRepository repository, IUserSession session) : base(repository) { }

            public async Task<List<Project>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await LoadAllEntities<Project>(cancellationToken);
            }
        }
    }
}
