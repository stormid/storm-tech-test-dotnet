
using Ardalis.Specification;

namespace Storm.TechTask.Core.ProjectAggregate.Specifications
{
    public class ToDoItemByProjectIdSpec : Specification<ToDoItem>, ISingleResultSpecification
    {
        public ToDoItemByProjectIdSpec(int projectId)
        {
            this.Query.Where(toDoItems => toDoItems.ProjectID == projectId);
        }
    }
}
