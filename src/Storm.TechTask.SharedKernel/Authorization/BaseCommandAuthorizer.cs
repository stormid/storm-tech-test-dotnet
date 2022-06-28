using Storm.TechTask.SharedKernel.Entities;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.SharedKernel.Authorization
{
    public class BaseCommandAuthorizer<TCommand> : IAuthorizer<TCommand>
    {
        private readonly IRepository _repository;

        protected BaseCommandAuthorizer(IRepository repository)
            => _repository = repository;

        public async virtual Task<AuthorizationResult> Authorize(IAppUser user, TCommand command, CancellationToken cancellationToken)
        {
            if (UserHasCorrectRole(user)
                && await CommandTargetIsInUserScope(user, command, cancellationToken)
                && await CommandTargetIsInCorrectState(command, cancellationToken)
                && await AppIsInCorrectState(cancellationToken))
            {
                return new AuthorizationResult(AuthorizationStatus.Success);
            }

            return new AuthorizationResult(AuthorizationStatus.Failure);
        }

        protected virtual bool UserHasCorrectRole(IAppUser user) => true;
        protected virtual Task<bool> CommandTargetIsInUserScope(IAppUser user, TCommand command, CancellationToken cancellationToken) => Task.FromResult(true);
        protected virtual Task<bool> CommandTargetIsInCorrectState(TCommand command, CancellationToken cancellationToken) => Task.FromResult(true);
        protected virtual Task<bool> AppIsInCorrectState(CancellationToken cancellationToken) => Task.FromResult(true);

        protected async Task<TEntity> LoadEntity<TEntity>(int entityId, CancellationToken cancellationToken) where TEntity : BaseEntity, IAggregateRoot
        {
            var entity = await _repository.GetByIdAsync<TEntity>(entityId, cancellationToken) ?? throw new EntityNotFoundException<TEntity>(entityId);
            return entity;
        }
    }
}
