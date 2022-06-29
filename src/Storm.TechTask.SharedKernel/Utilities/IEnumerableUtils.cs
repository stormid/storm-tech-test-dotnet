using Storm.TechTask.SharedKernel.Entities;

namespace Storm.TechTask.SharedKernel.Utilities
{
    public static class IEnumerableUtils
    {
        public static TChild GetChildEntity<TChild>(this IEnumerable<TChild> childEntities, int childEntityId) where TChild : BaseEntity
        {
            var childEntity = childEntities.SingleOrDefault(c => c.Id == childEntityId);

            if (childEntity is null)
            {
                throw new EntityNotFoundException<TChild>(childEntityId);
            }

            return childEntity;
        }
    }

}
