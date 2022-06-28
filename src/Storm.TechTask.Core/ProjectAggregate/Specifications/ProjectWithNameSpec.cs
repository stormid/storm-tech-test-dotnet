using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ardalis.Specification;

namespace Storm.TechTask.Core.ProjectAggregate.Specifications
{
    public class ProjectsWithNameSpec : Specification<Project>
    {
        public ProjectsWithNameSpec(string name)
        {
            this.Query.Where(p => p.Name == name);
        }
    }
}
