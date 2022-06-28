
using Ardalis.GuardClauses;

using Storm.TechTask.Core.ProjectAggregate.Exceptions;
using Storm.TechTask.Core.ProjectAggregate.Specifications;
using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.Services
{
    public class ProjectCreator : BaseDomainService
    {
        public ProjectCreator(IReadRepository repository) : base(repository)
        {
        }

        public async Task<Project> Create(string name, ProjectCategory category, bool internalOnly, CancellationToken cancellationToken)
        {
            await VerifyNameUnique(name, cancellationToken);
            Guard.Against.InvalidInput(category, nameof(category), c => c != ProjectCategory.NotSet);
            return new Project(name, category, internalOnly, ProjectStatus.Open);
        }

        private async Task VerifyNameUnique(string name, CancellationToken cancellationToken)
        {
            if (await CountEntities(new ProjectsWithNameSpec(name), cancellationToken) > 0)
            {
                throw new ProjectWithNameAlreadyExistsException(name);
            }
        }
    }

}
