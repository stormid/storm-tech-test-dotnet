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
    public interface IQuery : IAction
    {
    }

    public interface IQuery<out TResponse> : IRequest<TResponse>, IQuery
    {
    }

    public abstract class BaseQueryHandler
    {
        private readonly IRepository _repository;

        protected BaseQueryHandler(IRepository repository)
            => _repository = repository;

        protected async Task<TEntity> LoadEntity<TEntity>(int entityId, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            var entity = await _repository.GetByIdAsync<TEntity>(entityId, cancellationToken) ?? throw new EntityNotFoundException<TEntity>(entityId);
            return entity;
        }

        protected async Task<List<TEntity>> LoadAllEntities<TEntity>(CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            return await _repository.ListAsync<TEntity>(cancellationToken);
        }

        protected async Task<List<TEntity>> LoadEntitiesBySpec<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            return await _repository.ListAsync(specification, cancellationToken);
        }

        protected async Task<TEntity?> LoadEntityBySpec<TEntity, TSpec>(TSpec specification, CancellationToken cancellationToken) where TSpec : ISingleResultSpecification, ISpecification<TEntity> where TEntity : BaseEntity, IAggregateRoot
        {
            return await _repository.GetBySpecAsync<TEntity, TSpec>(specification, cancellationToken);
        }

    }
}
