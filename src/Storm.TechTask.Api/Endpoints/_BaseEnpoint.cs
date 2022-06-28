using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Api.Endpoints
{
    // This class has a quirky name coz subclasses don't directly refer to it - instead, they use the nested classes below.
    public abstract class _BaseEndpoint : EndpointBase, IEndpoint
    {
        protected readonly IMediator _mediator;
        public ILoggingService LoggingService { get; protected set; }
        public ISecurityService SecurityService { get; protected set; }

        protected _BaseEndpoint(IMediator mediator, ILoggingService loggingService, ISecurityService securityService)
        {
            _mediator = mediator;
            this.LoggingService = loggingService;
            this.SecurityService = securityService;
        }
    }

    // Mimic Ardalis means of exposing superclasses via fluent static nested classes.
    public static class BaseEndpoint
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResponse<TResponse> : _BaseEndpoint
            {
                public WithResponse(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
                {
                }

                public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
            }

            public abstract class WithoutResponse : _BaseEndpoint
            {
                public WithoutResponse(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
                {
                }

                public abstract Task<ActionResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResponse<TResponse> : _BaseEndpoint
            {
                public WithResponse(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
                {
                }

                public abstract Task<ActionResult<TResponse>> HandleAsync(CancellationToken cancellationToken = default);
            }

            public abstract class WithoutResponse : _BaseEndpoint
            {
                public WithoutResponse(IMediator mediator, ILoggingService loggingService, ISecurityService securityService) : base(mediator, loggingService, securityService)
                {
                }

                public abstract Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default);
            }
        }
    }

}
