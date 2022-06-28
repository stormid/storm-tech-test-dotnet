using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Infrastructure.Repository
{
    public class EfRepository : IRepository
    {
        protected readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync<T>(int id, CancellationToken cancellationToken) where T : BaseEntity
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<T?> GetBySpecAsync<T, TSpec>(TSpec specification, CancellationToken cancellationToken) where TSpec : ISingleResultSpecification, ISpecification<T> where T : BaseEntity
        {
            var specificationResult = ApplySpecification(specification);
            return await specificationResult.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult?> GetBySpecAsync<T, TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken) where T : BaseEntity
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync<T>(CancellationToken cancellationToken) where T : BaseEntity
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : BaseEntity
        {
            var specificationResult = ApplySpecification(specification);
            return await specificationResult.ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken) where T : BaseEntity, IAggregateRoot
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync<T>(ISpecification<T> specification, CancellationToken cancellationToken) where T : BaseEntity
        {
            return ApplySpecification(specification, true).CountAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task<int> CountAsync<T>(CancellationToken cancellationToken) where T : BaseEntity
        {
            return _dbContext.Set<T>().CountAsync(cancellationToken);
        }

        public void Detach<T>(T entity) where T : BaseEntity, IAggregateRoot
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        private IQueryable<T> ApplySpecification<T>(ISpecification<T> specification, bool evaluateCriteriaOnly = false) where T : BaseEntity
        {
            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), specification, evaluateCriteriaOnly);
        }

        private IQueryable<TResult> ApplySpecification<T, TResult>(ISpecification<T, TResult> specification) where T : BaseEntity
        {
            if (specification is null)
            {
                throw new ArgumentNullException("Specification is required");
            }

            if (specification.Selector is null)
            {
                throw new SelectorNotFoundException();
            }

            return SpecificationEvaluator.Default.GetQuery(_dbContext.Set<T>().AsQueryable(), specification);
        }
    }
}
