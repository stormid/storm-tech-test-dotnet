using MediatR;

using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.SharedKernel.Authorization
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ISecurityService _securityService;
        private readonly IAuthorizer<TRequest> _authorizer;
        private readonly ILoggingService _loggingService;

        public AuthorizationBehaviour(ISecurityService securityService, IAuthorizer<TRequest> authorizer, ILoggingService loggingService)
        {
            _securityService = securityService;
            _authorizer = authorizer;
            _loggingService = loggingService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if ((await _authorizer.Authorize(_securityService.CurrentUser, request, cancellationToken)).IsAuthorized)
            {
                _loggingService.SecurityLogger.Debug("Request authorized");
                return await next();
            }
            else
            {
                _loggingService.SecurityLogger.Warning("Request not authorized");
                throw new NotAuthorizedException();
            }
        }
    }
}
