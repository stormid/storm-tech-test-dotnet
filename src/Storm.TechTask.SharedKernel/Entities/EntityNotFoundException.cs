using Ardalis.Specification;

namespace Storm.TechTask.SharedKernel.Entities
{
    public class EntityNotFoundException<T> : Exception where T : BaseEntity
    {
        public EntityNotFoundException(int id) : base($"Failed to load a {typeof(T).Name} with id {id}") { }

        public EntityNotFoundException(ISpecification<T> specification) : base($"Failed to load entity using specification {specification.GetType().Name}") { }
    }
}
