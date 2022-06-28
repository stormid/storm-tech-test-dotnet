using Ardalis.Specification;

using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.SharedKernel.Entities
{
    public abstract class BaseDomainService
    {
        private readonly IReadRepository _repository;

        protected BaseDomainService(IReadRepository repository)
        {
            _repository = repository;
        }

        protected async Task<TEntity> LoadEntity<TEntity>(int entityId, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            var entity = await _repository.GetByIdAsync<TEntity>(entityId, cancellationToken) ?? throw new EntityNotFoundException<TEntity>(entityId);
            return entity;
        }

        protected async Task<List<TEntity>> LoadEntities<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            return await _repository.ListAsync(specification, cancellationToken);
        }

        protected async Task<int> CountEntities<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            return await _repository.CountAsync(specification, cancellationToken);
        }
    }
}
