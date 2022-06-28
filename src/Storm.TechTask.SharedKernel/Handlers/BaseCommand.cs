using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ardalis.Specification;

using MediatR;

using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.SharedKernel.Handlers
{
    public interface ICommand : IAction
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand
    {
    }

    public abstract class BaseCommandHandler
    {
        protected readonly IRepository _repository;

        protected BaseCommandHandler(IRepository repository)
            => _repository = repository;

        protected async Task<TEntity> LoadEntity<TEntity>(int entityId, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            return await _repository.GetByIdAsync<TEntity>(entityId, cancellationToken) ?? throw new EntityNotFoundException<TEntity>(entityId);
        }

        protected async Task<TEntity> LoadEntityBySpec<TEntity, TSpec>(TSpec specification, CancellationToken cancellationToken)
            where TEntity : BaseEntity, IAggregateRoot where TSpec : ISingleResultSpecification, ISpecification<TEntity>
        {
            return await _repository.GetBySpecAsync<TEntity, TSpec>(specification, cancellationToken) ?? throw new EntityNotFoundException<TEntity>(specification);
        }

        protected async Task<TEntity> CreateEntity<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            return await _repository.AddAsync(entity, cancellationToken);
        }

        protected async Task UpdateEntity<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            await _repository.UpdateAsync(entity, cancellationToken);
        }

        protected async Task DeleteEntity<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            await _repository.DeleteAsync(entity, cancellationToken);
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
