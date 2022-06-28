using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.Core.ProjectAggregate.Exceptions
{
    public class ProjectWithNameAlreadyExistsException : BusinessRuleException
    {
        public ProjectWithNameAlreadyExistsException(string name) : base($"A Project with name '{name}' already exists")
        {
        }
    }
}
