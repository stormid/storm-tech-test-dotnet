namespace Storm.TechTask.SharedKernel.Authorization
{
    public enum AuthorizationStatus
    {
        Success,
        Failure
    }

    public class AuthorizationResult
    {
        public AuthorizationStatus Status { get; private set; }

        public bool IsAuthorized => Status == AuthorizationStatus.Success;

        public AuthorizationResult(AuthorizationStatus status)
        {
            Status = status;
        }
    }

    public interface IAuthorizer<TRequest> //: IAuthorizer where TCommand : class
    {
        Task<AuthorizationResult> Authorize(IAppUser user, TRequest request, CancellationToken cancellationToken);
    }

}
