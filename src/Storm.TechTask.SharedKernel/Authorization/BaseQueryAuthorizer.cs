namespace Storm.TechTask.SharedKernel.Authorization
{
    public class BaseQueryAuthorizer<TQuery> : IAuthorizer<TQuery>
    {
        public async virtual Task<AuthorizationResult> Authorize(IAppUser user, TQuery query, CancellationToken cancellationToken)
        {
            if (UserHasCorrectRole(user)
                && await QueryFiltersAreInUserScope(user, query, cancellationToken)
                && await AppIsInCorrectState(cancellationToken))
            {
                return new AuthorizationResult(AuthorizationStatus.Success);
            }
            else
            {
                return new AuthorizationResult(AuthorizationStatus.Failure);
            }
        }

        protected virtual bool UserHasCorrectRole(IAppUser user) => true;
        protected virtual Task<bool> QueryFiltersAreInUserScope(IAppUser user, TQuery query, CancellationToken cancellationToken) => Task.FromResult(true);
        protected virtual Task<bool> AppIsInCorrectState(CancellationToken cancellationToken) => Task.FromResult(true);
    }

}
