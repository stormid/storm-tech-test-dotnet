using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.Core.ProjectAggregate.Exceptions;
using Storm.TechTask.Core.ProjectAggregate.Specifications;
using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Core.ProjectAggregate.Services
{
    public interface IProjectUniquenessChecker
    {
        Task VerifyNameUnique(Project project, string name, CancellationToken cancellationToken);
    }

    public class ProjectUniquenessChecker : BaseDomainService, IProjectUniquenessChecker
    {
        public ProjectUniquenessChecker(IReadRepository repository) : base(repository)
        {
        }

        public async Task VerifyNameUnique(Project project, string name, CancellationToken cancellationToken)
        {
            var existingProjects = await LoadEntities(new ProjectsWithNameSpec(name), cancellationToken);
            if ((existingProjects.Count == 1) && (existingProjects[0].Id != project.Id))
            {
                throw new ProjectWithNameAlreadyExistsException(name);
            }
        }
    }

}
